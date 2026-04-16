# Plan de Refactorización - TaskTrackerCLI

## Análisis del Proyecto

### Estructura Actual
```
TaskTrackerCLI/
├── Commands/
│   ├── AddCommand.cs
│   ├── UpdateCommand.cs
│   ├── RemoveCommand.cs
│   └── ListCommand.cs
├── Models/
│   ├── TaskModel.cs
│   ├── Config.cs
│   └── AppDataJsonModel.cs
├── Program.cs
└── TaskTrackerCLI.csproj
```

---

## Problemas Identificados

### 🔴 Críticos
| # | Problema | Ubicación |
|---|----------|-----------|
| 1 | `RemoveCommand.Handle()` está vacío - no elimina tareas | Commands/RemoveCommand.cs |
| 2 | `UpdateCommand.Execute()` solo lista, no actualiza | Commands/UpdateCommand.cs:31-58 |
| 3 | `mark-in-progress` handler vacío | Program.cs:74-86 |
| 4 | `mark-completed` handler vacío | Program.cs:88-102 |

### 🟠 Altos
| # | Problema | Ubicación |
|---|----------|-----------|
| 5 | `LoadTasks()` duplicado en 3 commands | AddCommand, ListCommand, UpdateCommand |
| 6 | Lógica de listado triplicada (done/todo/inProgress) | ListCommand.cs:21-65 |
| 7 | Modelo de datos incorrecto (AppDataJsonModel vs JSON real) | Models/AppDataJsonModel.cs |
| 8 | JSON en `AppContext.BaseDirectory` (carpeta de build) | Program.cs:14-16 |

### 🟡 Medios
| # | Problema | Ubicación |
|---|----------|-----------|
| 9 | Propiedades en camelCase (`id`, `status`) | Models/TaskModel.cs |
| 10 | Campos públicos en lugar de propiedades | Commands/*.cs |
| 11 | Imports no utilizados | Varios archivos |
| 12 | Exceso de líneas vacías | Program.cs |

### 🟢 Bajos
| # | Problema | Ubicación |
|---|----------|-----------|
| 13 | `Config.NombreApp` en español | Models/Config.cs |
| 14 | Clase `Config` no utilizada | Models/Config.cs |
| 15 | Sin tests unitarios | Proyecto |

---

## Plan de Refactorización

### Fase 1: Funcionalidad Crítica
**Objetivo:** Completar comandos no implementados

- [ ] **1.1** Implementar `RemoveCommand.Handle()` para eliminar tareas por ID
- [ ] **1.2** Implementar `UpdateCommand.Execute()` para actualizar descripción
- [ ] **1.3** Implementar `HandleMarkInProgress()` en Program.cs
- [ ] **1.4** Implementar `HandleMarkCompleted()` en Program.cs

### Fase 2: Extracción de Responsabilidades
**Objetivo:** Eliminar duplicación y mejorar cohesión

- [ ] **2.1** Crear `TaskRepository.cs` para operaciones JSON centralizadas
  ```csharp
  public class TaskRepository
  {
      public List<TaskItem> GetAll();
      public TaskItem? GetById(int id);
      public void Add(TaskItem task);
      public void Update(TaskItem task);
      public void Remove(int id);
      public void Save();
  }
  ```
- [ ] **2.2** Extraer enum `TaskStatus` a archivo propio
- [ ] **2.3** Crear `TaskItem` (renombrar de TaskModel, propiedades PascalCase)
- [ ] **2.4** Refactorizar `ListCommand` con método genérico de filtrado

### Fase 3: Modelo de Datos
**Objetivo:** Corregir desajuste entre modelo y JSON

- [ ] **3.1** Simplificar estructura JSON a lista directa de tareas
- [ ] **3.2** Eliminar clase `Config` (no usada) o documentar su propósito
- [ ] **3.3** Validar longitud de descripción

### Fase 4: Almacenamiento
**Objetivo:** Usar directorio de datos de aplicación

- [ ] **4.1** Mover JSON a directorio de datos de usuario
  ```csharp
  var appDataPath = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
      "TaskTrackerCLI",
      "tasks.json"
  );
  ```

### Fase 5: Manejo de Errores
**Objetivo:** Aplicaciones robustas

- [ ] **5.1** Agregar try-catch en operaciones de archivo
- [ ] **5.2** Validar existencia de tarea antes de actualizar/eliminar
- [ ] **5.3** Manejar JSON corrupto o archivo no existente

### Fase 6: Estilo y Convenciones
**Objetivo:** Consistencia con convenciones C#

- [ ] **6.1** Renombrar propiedades a PascalCase
- [ ] **6.2** Convertir campos públicos a propiedades
- [ ] **6.3** Eliminar imports no utilizados
- [ ] **6.4** Limpiar espaciado excesivo
- [ ] **6.5** Agregar null-coalescing para argumentos opcionales

### Fase 7: Tests
**Objetivo:** Garantizar calidad

- [ ] **7.1** Crear proyecto de tests (xUnit o NUnit)
- [ ] **7.2** Tests para `TaskRepository`
- [ ] **7.3** Tests para comandos principales

### Fase 8: Documentación
**Objetivo:** Mantenibilidad futura

- [ ] **8.1** Agregar README.md con uso del CLI
- [ ] **8.2** Agregar XML docs en clases públicas

---

## Estructura Objetivo

```
TaskTrackerCLI/
├── Commands/
│   ├── AddCommand.cs
│   ├── ListCommand.cs
│   ├── UpdateCommand.cs
│   ├── RemoveCommand.cs
│   ├── MarkInProgressCommand.cs
│   └── MarkDoneCommand.cs
├── Models/
│   ├── TaskItem.cs          # (renamed from TaskModel)
│   └── TaskStatus.cs        # (extracted enum)
├── Services/
│   └── TaskRepository.cs    # (NEW - centralized JSON ops)
├── Infrastructure/
│   └── FileService.cs       # (NEW - file path resolution)
├── Program.cs
└── TaskTrackerCLI.csproj
```

---

## Orden de Implementación Recomendada

```
1. TaskStatus.cs (enum) → 2. TaskItem.cs (modelo) → 3. TaskRepository.cs
→ 4. Refactorizar comandos existentes → 5. Completar comandos faltantes
→ 6. Tests → 7. Docs
```

---

## Notas

- El proyecto usa `.NET 10.0` con `System.CommandLine` 2.0.5
- Considerar async/await para operaciones de archivo futuras
- Para multi-plataforma, usar `Path.Combine` con `Environment.SpecialFolder.ApplicationData`
