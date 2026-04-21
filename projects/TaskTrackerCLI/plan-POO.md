# Plan de Refactorización - Principios POO y Patrones de Diseño

## Análisis del Código Actual

### Estructura Actual
```
Commands/
├── AddCommand.cs        (~64 líneas)
├── UpdateCommand.cs     (~142 líneas) ← clase anidada problemática
├── RemoveCommand.cs
└── ListCommand.cs

Repositories/
├── Interfaces/ITaskRepository.cs
├── JsonTaskRepository.cs
└── JsonDataSource.cs

Program.cs               ← formato extraño, instanciación manual
```

---

## Problemas Identificados por Principio/Patrón

### SOLID

| Principio | Problema | Ubicación |
|-----------|----------|-----------|
| **SRP** | UpdateCommand tiene 3 responsabilidades: update, mark-completed, mark-in-progress | UpdateCommand.cs |
| **OCP** | Comandos no extensibles sin modificar código existente | Commands/*.cs |
| **LSP** | MarkInProgressCommand/MarkCompletedCommand heredan comportamiento de forma inconsistente | UpdateCommand.cs:66,104 |
| **ISP** | ITaskRepository tiene métodos de impresión (PrintTask, PrintTasksByStatus) | ITaskRepository.cs:13-14 |
| **DIP** | Dependencia directa de JsonTaskRepository en Program.cs | Program.cs:17 |

### DRY (Don't Repeat Yourself)

| Código Duplicado | Ubicaciones |
|------------------|-------------|
| Carga de tareas (`GetAllTasks()`) | AddCommand, UpdateCommand, ListCommand, RemoveCommand |
| Guardado de tareas (`SaveTask()`) | AddCommand, UpdateCommand, RemoveCommand, MarkCommands |
| Parsing de ID (`int.TryParse`) | UpdateCommand, RemoveCommand, MarkCommands |
| Validación de descripción | AddCommand, UpdateCommand (constante duplicada) |
| Configuración de Argument | Todos los comandos |
| Manejo de Execute/Handle | Todos los comandos |

### Code Smells

| Smell | Descripción |
|-------|-------------|
| God Class | UpdateCommand con clases anidadas |
| Feature Envy | Comandos acceden directamente al modelo en lugar de usar el repository |
| Shotgun Surgery | Cambios en TaskRepository requieren cambios en múltiples comandos |
| Parallel Inheritance | Cada nuevo comando requiere misma estructura |

---

## Plan de Refactorización por Patrón

### Patrón 1: Abstract Base Command (Template Method + Inheritance)

**Objetivo**: Eliminar código duplicado en comandos

```csharp
// Commands/Base/CommandBase.cs
public abstract class CommandBase
{
    protected readonly ITaskRepository _repository;
    protected readonly Argument<string> _argument;
    protected readonly Command _command;

    protected CommandBase(ITaskRepository repository, string name, string description)
    {
        _repository = repository;
        _command = new Command(name, description);
        _argument = CreateArgument();
        _command.Arguments.Add(_argument);
        _command.SetAction(Handle);
    }

    protected abstract Argument<string> CreateArgument();
    protected abstract Task ExecuteCore(string? value);

    public async Task Handle(ParseResult parseResult)
    {
        string? value = parseResult.GetValue(_argument);
        await ExecuteCore(value);
    }

    public Command GetCommand() => _command;
}
```

**Comandos resultantes** (~30 líneas cada uno):
- AddCommand : CommandBase
- RemoveCommand : CommandBase

---

### Patrón 2: Command Handler con Funciones

**Objetivo**: Abstraer la lógica de ejecución

```csharp
// Commands/TaskCommandHandler.cs
public class TaskCommandHandler
{
    private readonly ITaskRepository _repository;

    public TaskItem CreateTask(string description) => new TaskItem { ... };
    public void UpdateTask(TaskItem task, Action<TaskItem> updateAction) => updateAction(task);
    public void DeleteTask(int id) => _repository.RemoveTask(id);
    public IEnumerable<TaskItem> GetTasks() => _repository.GetAllTasks().Tasks;
}
```

---

### Patrón 3: Interfaz Segregada (ISP)

**Objetivo**: Separar responsabilidades del Repository

```csharp
// Repositories/Interfaces/ITaskRepository.cs
public interface ITaskRepository
{
    TaskItem? GetById(int id);
    IEnumerable<TaskItem> GetAll();
    void Add(TaskItem task);
    void Update(TaskItem task);
    void Remove(int id);
    Task SaveChangesAsync();
}

// Repositories/Interfaces/ITaskOutput.cs (NEW)
public interface ITaskOutput
{
    void Print(TaskItem task);
    void PrintAll(IEnumerable<TaskItem> tasks);
    void PrintByStatus(IEnumerable<TaskItem> tasks, TaskStatus status);
}
```

---

### Patrón 4: Strategy para Almacenamiento

**Objetivo**: Permitir diferentes backends de almacenamiento

```csharp
// Repositories/Interfaces/ITaskStorage.cs
public interface ITaskStorage
{
    Task<IEnumerable<TaskItem>> LoadAsync();
    Task SaveAsync(IEnumerable<TaskItem> tasks);
}

// JsonStorage : ITaskStorage
// InMemoryStorage : ITaskStorage (para tests)
// SqliteStorage : ITaskStorage (futuro)
```

---

### Patrón 5: Factory para Comandos

**Objetivo**: Centralizar creación de comandos

```csharp
// Commands/CommandFactory.cs
public class CommandFactory
{
    private readonly ITaskRepository _repository;

    public AddCommand CreateAddCommand() => new AddCommand(_repository);
    public RemoveCommand CreateRemoveCommand() => new RemoveCommand(_repository);
    // ...
}
```

---

### Patrón 6: Service Layer

**Objetivo**: Separar lógica de negocio del acceso a datos

```
Services/
├── ITaskService.cs
│   └── AddTask(description) → valida, crea, guarda
│   └── UpdateTask(id, description) → busca, actualiza, guarda
│   └── RemoveTask(id) → busca, elimina, guarda
│   └── GetTasks(filter?) → filtra si aplica
│
├── TaskRepository (solo acceso a datos)
│   └── GetAll(), Add(), Update(), Remove(), Save()
```

---

### Patrón 7: Dependency Injection

**Objetivo**: Desacoplar dependencias

```csharp
// Services/ServiceConfiguration.cs
public static class ServiceConfiguration
{
    public static IServiceProvider Configure()
    {
        var services = new ServiceCollection();
        
        services.AddSingleton<ITaskStorage, JsonStorage>();
        services.AddSingleton<ITaskRepository, JsonTaskRepository>();
        services.AddSingleton<ITaskService, TaskService>();
        services.AddTransient<AddCommand>();
        services.AddTransient<RemoveCommand>();
        // ...
        
        return services.BuildServiceProvider();
    }
}
```

---

## Estructura Objetivo

```
TaskTrackerCLI/
├── Commands/
│   ├── Base/
│   │   └── CommandBase.cs           # Clase abstracta
│   ├── AddCommand.cs
│   ├── RemoveCommand.cs
│   ├── ListCommand.cs
│   ├── UpdateCommand.cs
│   ├── MarkCompletedCommand.cs      # Archivos separados
│   ├── MarkInProgressCommand.cs
│   └── CommandFactory.cs
│
├── Services/
│   ├── Interfaces/
│   │   └── ITaskService.cs
│   └── TaskService.cs
│
├── Repositories/
│   ├── Interfaces/
│   │   ├── ITaskRepository.cs       # Segregada
│   │   └── ITaskStorage.cs         # Strategy
│   ├── JsonTaskRepository.cs
│   └── JsonStorage.cs
│
├── Infrastructure/
│   └── FileService.cs               # Para path
│
├── Models/
│   ├── TaskItem.cs
│   └── TaskStatus.cs
│
└── Program.cs                       # Configuración DI
```

---

## Orden de Implementación

### Fase 1: Refactorización de Repository
- [ ] 1.1 Crear ITaskStorage (Strategy Pattern)
- [ ] 1.2 Segregar ITaskRepository (ISP)
- [ ] 1.3 Eliminar métodos de impresión del Repository

### Fase 2: Command Base
- [ ] 2.1 Crear CommandBase abstracta
- [ ] 2.2 Refactorizar AddCommand
- [ ] 2.3 Refactorizar RemoveCommand

### Fase 3: Service Layer
- [ ] 3.1 Crear ITaskService
- [ ] 3.2 Implementar TaskService (lógica de negocio)
- [ ] 3.3 Actualizar comandos para usar Service

### Fase 4: Extraer Comandos Anidados
- [ ] 4.1 Mover MarkCompletedCommand a archivo propio
- [ ] 4.2 Mover MarkInProgressCommand a archivo propio
- [ ] 4.3 Refactorizar UpdateCommand

### Fase 5: Factory
- [ ] 5.1 Crear CommandFactory
- [ ] 5.2 Actualizar Program.cs

### Fase 6: Dependency Injection
- [ ] 6.1 Configurar DI Container
- [ ] 6.2 Refactorizar Program.cs

---

## Beneficios Esperados

| Métrica | Antes | Después |
|---------|-------|---------|
| Líneas por comando | ~60-140 | ~20-30 |
| Código duplicado | ~40% | ~5% |
| Acoplamiento | Alto | Bajo |
| Testabilidad | Difícil | Fácil (mocking) |
| Extensibilidad | Difícil | Agregar comando sin modificar existente |

---

## Notas

- Usar `Microsoft.Extensions.DependencyInjection` para DI
- Considerar `FluentValidation` para validaciones
- Mantener backwards compatibility con CLI actual
