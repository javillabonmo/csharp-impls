# Plan de Refactorización - TaskTrackerCLI

## Estado Actual: Parcialmente Completado

### Estructura Actual (Actualizada)
```
TaskTrackerCLI/
├── Commands/
│   ├── AddCommand.cs
│   ├── ListCommand.cs
│   ├── UpdateCommand.cs        # Incluye MarkInProgressCommand y MarkCompletedCommand
│   └── RemoveCommand.cs
├── Models/
│   ├── TaskItem.cs             # (anteriormente TaskModel, con PascalCase)
│   ├── TaskStatus.cs           # (enum extraído)
│   ├── Config.cs
│   └── AppDataJsonModel.cs
├── Repositories/
│   ├── Interfaces/
│   │   └── ITaskRepository.cs
│   ├── JsonTaskRepository.cs
│   └── JsonDataSource.cs
├── Program.cs
└── TaskTrackerCLI.csproj
```

---

## Progreso: Completado vs Pendiente

### ✅ COMPLETADOS

#### Fase 1: Funcionalidad Crítica
| # | Item | Estado |
|---|------|--------|
| 1.1 | `RemoveCommand.Handle()` - eliminar tareas por ID | ✅ Implementado |
| 1.2 | `UpdateCommand.Execute()` - actualizar descripción | ✅ Implementado |
| 1.3 | `mark-in-progress` handler | ✅ Implementado como clase anidada |
| 1.4 | `mark-completed` handler | ✅ Implementado como clase anidada |

#### Fase 2: Extracción de Responsabilidades
| # | Item | Estado |
|---|------|--------|
| 2.1 | `TaskRepository` para operaciones JSON | ✅ ITaskRepository, JsonTaskRepository, JsonDataSource |
| 2.4 | Refactorizar ListCommand con filtrado genérico | ✅ Usa `PrintTasksByStatus()` del repositorio |

#### Fase 5: Manejo de Errores
| # | Item | Estado |
|---|------|--------|
| 5.1 | try-catch en operaciones de archivo | ✅ En JsonDataSource.cs |
| 5.3 | Manejar JSON corrupto | ✅ JsonDataSource.cs:25-35 |

---

### ⏳ PENDIENTES

#### Fase 2: Extracción de Responsabilidades
| # | Item | Prioridad |
|---|------|-----------|
| 2.2 | Extraer enum `TaskStatus` a archivo propio | ✅ Completado |
| 2.3 | Renombrar `TaskModel` → `TaskItem` con PascalCase | ✅ Completado |

#### Fase 3: Modelo de Datos
| # | Item | Prioridad |
|---|------|-----------|
| 3.1 | Simplificar estructura JSON (eliminar Config si no se usa) | Baja |
| 3.2 | Eliminar clase `Config` o documentar propósito | Baja |
| 3.3 | Validar longitud de descripción | Baja |

#### Fase 4: Almacenamiento
| # | Item | Prioridad |
|---|------|-----------|
| 4.1 | Mover JSON a directorio de datos de usuario | Alta |
| | *Pendiente: usar `Environment.SpecialFolder.ApplicationData`* | |

#### Fase 5: Manejo de Errores
| # | Item | Prioridad |
|---|------|-----------|
| 5.2 | Validar existencia de tarea antes de actualizar/eliminar | Media |

#### Fase 6: Estilo y Convenciones
| # | Item | Prioridad |
|---|------|-----------|
| 6.1 | Propiedades a PascalCase (`Id`, `Status`, `CreatedAt`, `UpdatedAt`) | Media |
| 6.2 | Campos públicos → propiedades (ya mayormente hecho) | Baja |
| 6.3 | Eliminar imports no utilizados | Baja |
| 6.4 | Limpiar espaciado excesivo (Program.cs líneas vacías) | Baja |
| 6.5 | Agregar null-coalescing para argumentos | Baja |

#### Fase 7: Tests
| # | Item | Prioridad |
|---|------|-----------|
| 7.1 | Crear proyecto de tests (xUnit/NUnit) | Alta |
| 7.2 | Tests para `TaskRepository` | Alta |
| 7.3 | Tests para comandos principales | Media |

#### Fase 8: Documentación
| # | Item | Prioridad |
|---|------|-----------|
| 8.1 | Agregar README.md | Media |
| 8.2 | Agregar XML docs en clases públicas | Baja |

---

## Problemas Originales - Estado Final

| # | Problema Original | Estado |
|---|-------------------|--------|
| 1 | `RemoveCommand.Handle()` vacío | ✅ Corregido |
| 2 | `UpdateCommand.Execute()` no actualizaba | ✅ Corregido |
| 3 | `mark-in-progress` handler vacío | ✅ Corregido |
| 4 | `mark-completed` handler vacío | ✅ Corregido |
| 5 | `LoadTasks()` duplicado | ✅ Corregido (via Repository) |
| 6 | Lógica de listado triplicada | ✅ Corregido (PrintTasksByStatus) |
| 7 | Modelo vs JSON real | ⚠️ Verificar |
| 8 | JSON en BaseDirectory | ⏳ Pendiente |

---

## Siguientes Pasos Recomendados

1. **Inmediato**: Mover JSON a ApplicationData (Fase 4.1) - Alta prioridad
2. **Tests**: Crear proyecto de tests (Fase 7) - Alta prioridad  
3. **Limpieza**: PascalCase y eliminar code smells menores (Fase 6)
4. **Documentación**: README.md (Fase 8.1)

---

## Notas

- El proyecto ya tiene arquitectura de Repository implementada
- Los comandos principales (Add, List, Update, Remove, mark-in-progress, mark-completed) funcionan
- Quedan pendientes: ubicación del archivo, tests, convenciones de nombres, y documentación
