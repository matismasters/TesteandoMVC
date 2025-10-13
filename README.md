# TesteandoMVC - Proyecto de Demostración ASP.NET Core MVC

## 📋 Descripción del Proyecto

**TesteandoMVC** es una aplicación web de demostración desarrollada en ASP.NET Core MVC con .NET 9.0. El proyecto está diseñado para mostrar conceptos fundamentales del desarrollo web con el patrón MVC (Model-View-Controller), incluyendo servicios de negocio, inyección de dependencias, testing unitario e integración.

## 🏗️ Estructura del Proyecto

```
TesteandoMVC/
├── TesteandoMVC.sln                    # Archivo de solución de Visual Studio
├── TesteandoMVC.Web/                   # Proyecto principal de la aplicación web
│   ├── Controllers/
│   │   └── HomeController.cs           # Controlador principal
│   ├── Models/
│   │   └── ErrorViewModel.cs           # Modelo para manejo de errores
│   ├── Services/
│   │   └── SimpleService.cs            # Servicio de lógica de negocio
│   ├── Views/
│   │   ├── Home/
│   │   │   ├── Index.cshtml           # Página principal
│   │   │   ├── Login.cshtml           # Formulario de login
│   │   │   ├── NumeroAleatorio.cshtml # Generador de números
│   │   │   └── Privacy.cshtml         # Página de privacidad
│   │   └── Shared/
│   │       └── _Layout.cshtml         # Layout principal
│   ├── wwwroot/                       # Archivos estáticos (CSS, JS, imágenes)
│   ├── Program.cs                     # Punto de entrada de la aplicación
│   ├── appsettings.json              # Configuración de la aplicación
│   └── TesteandoMVC.Web.csproj       # Archivo de proyecto
└── TesteandoMVC.Tests/                # Proyecto de pruebas unitarias
    ├── HomeControllerTests.cs         # Tests del controlador Home
    ├── UnitTest1.cs                   # Tests de integración
    └── TesteandoMVC.Tests.csproj      # Archivo de proyecto de tests
```

## ⚡ Funcionalidades Principales

### 1. **Página Principal (Index)**
- **Funcionalidad**: Muestra el estado del servicio basado en la hora actual
- **Lógica**: Si la hora actual es par, muestra "¡El servicio funciona correctamente!", si es impar muestra "¡El servicio no está funcionando!"
- **Navegación**: Proporciona enlaces a las demás funcionalidades

### 2. **Generador de Números Aleatorios**
- **Funcionalidad**: Genera un número aleatorio entre 1 y 100
- **Uso**: Cada visita a `/Home/NumeroAleatorio` genera un nuevo número

### 3. **Sistema de Login**
- **Funcionalidad**: Formulario de autenticación básica
- **Credenciales válidas**: 
  - Usuario: `don_correcto`
  - Contraseña: `iatusabes`
- **Validación**: Muestra mensajes de éxito o error según las credenciales

### 4. **Página de Privacidad**
- **Funcionalidad**: Página estática de información de privacidad

### 5. **🧪 Demo de Test Doubles (SIMPLIFICADO)**
- **Funcionalidad**: Endpoint simple que demuestra los 5 tipos de Test Doubles  
- **Ubicación**: `/Home/TestDoubles`
- **Un solo servicio**: `ITestDoublesService` con métodos específicos para cada tipo
- **Características**:
  - **DUMMY**: Logger pasado pero no usado
  - **STUB**: Descuentos con valores predefinidos  
  - **FAKE**: Almacenamiento simple en memoria
  - **MOCK**: Notificaciones para verificar llamadas
  - **SPY**: Validación con logs inspeccionables
- **Interfaz simple**: Una sola vista que muestra todos los resultados

## 🔧 Tecnologías y Arquitectura

### **Framework y Versión**
- **ASP.NET Core MVC 9.0** - Framework web de Microsoft
- **C# 12** - Lenguaje de programación
- **.NET 9.0** - Runtime y framework base

### **Arquitectura MVC**

#### **Controllers (Controladores)**
- **`HomeController`**: Maneja todas las rutas principales de la aplicación
  - `Index()`: Página principal con estado del servicio
  - `Privacy()`: Página de privacidad
  - `NumeroAleatorio()`: Generador de números aleatorios
  - `Login()`: Muestra formulario de login
  - `ValidarLogin(string usuario, string password)`: Procesa login por POST
  - **`TestDoubles(string input)`**: 🧪 **Demo Simple de Test Doubles**
    - Un solo endpoint que usa todos los tipos de Test Doubles
    - Recibe un parámetro de texto para demostración
    - Muestra resultados de DUMMY, STUB, FAKE, MOCK y SPY
  - `Error()`: Manejo de errores

#### **Models (Modelos)**
- **`ErrorViewModel`**: Modelo para mostrar información de errores con RequestId

#### **Views (Vistas)**
- **Razor Pages** con layout responsivo usando Bootstrap 5
- **Layout compartido** (`_Layout.cshtml`) con navegación y estructura común
- **Vistas específicas** para cada acción del controlador

#### **Services (Servicios)**

##### **Servicios Originales**
- **`ISimpleService`** (Interface): Define el contrato del servicio
- **`SimpleService`** (Implementación): Contiene la lógica de negocio
  - `HoraEsPar()`: Determina si la hora actual es par
  - `NumeroAleatorio()`: Genera número aleatorio 1-100
  - `ValidarUsuario(usuario, password)`: Valida credenciales

##### **🧪 Servicio Simple para Demo de Test Doubles**
- **`ITestDoublesService` / `TestDoublesService`**: 
  - **Un solo servicio** que demuestra todos los tipos de Test Doubles
  - `ProcesarTexto(logger, texto)`: **DUMMY** - Logger se pasa pero no se usa
  - `CalcularDescuento(tipo)`: **STUB** - Retorna valores predefinidos
  - `GuardarDato/ObtenerDato`: **FAKE** - Implementación real en memoria
  - `EnviarNotificacion(mensaje)`: **MOCK** - Para verificar llamadas en tests
  - `ValidarYProcesar(input)`: **SPY** - Con logs que se inspeccionan en tests

### **Inyección de Dependencias**
- Configurada en `Program.cs` con `builder.Services.AddScoped<ISimpleService, SimpleService>()`
- Permite testear fácilmente mediante mocking

### **Configuración de la Aplicación**
```csharp
// Program.cs - Configuración principal
var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISimpleService, SimpleService>();

var app = builder.Build();

// Pipeline de middleware
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

## 🧪 Testing y Calidad de Código

### **Framework de Testing**
- **xUnit** - Framework de testing principal
- **FluentAssertions** - Aserciones más expresivas
- **Moq** - Framework de mocking para crear objetos simulados
- **Microsoft.AspNetCore.Mvc.Testing** - Testing de integración

### **Tipos de Tests Implementados**

#### **Tests de Integración (`UnitTest1.cs`)**
```csharp
[Theory]
[InlineData("/")]
[InlineData("/Home/Privacy")]
public async Task Pages_Return_Status200(string url)
{
    // Verifica que las páginas principales retornen HTTP 200
}
```

#### **Tests Unitarios con Mocking (`HomeControllerTests.cs`)**
```csharp
[Fact]
public async Task Index_CuandoElServicioDevuelveTrue_MuestraMensajeDeExito()
{
    // Arrange: Configura mock del servicio
    var mockService = new Mock<ISimpleService>();
    mockService.Setup(x => x.HoraEsPar()).Returns(true);
    
    // Act: Ejecuta la acción
    var response = await client.GetAsync("/");
    
    // Assert: Verifica el resultado esperado
    Assert.Contains("¡El servicio funciona correctamente!", content);
}
```

### **Cobertura de Testing**
- ✅ **Testing de integración** de páginas principales
- ✅ **Testing unitario** con mocking de servicios
- ✅ **Testing de comportamiento** según estado del servicio
- ✅ **Ejemplos de mocking** para diferentes escenarios
- ✅ **🧪 Demo completo de Test Doubles**: 
  - **DUMMY Objects**: Objetos pasados pero no usados
  - **STUB**: Servicios que retornan valores predefinidos
  - **FAKE**: Implementaciones reales pero simplificadas
  - **MOCK**: Verificación de llamadas y parámetros
  - **SPY**: Inspección detallada de interacciones y secuencias

## 🚀 Cómo Ejecutar el Proyecto

### **Requisitos Previos**
- **.NET 9.0 SDK** o superior
- **Visual Studio 2022** (opcional) o **VS Code**
- **Git** para clonar el repositorio

### **Pasos para Ejecutar**

1. **Clonar o navegar al proyecto**
   ```bash
   cd /ruta/al/proyecto/TesteandoMVC
   ```

2. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

3. **Compilar el proyecto**
   ```bash
   dotnet build
   ```

4. **Ejecutar la aplicación**
   ```bash
   cd TesteandoMVC.Web
   dotnet run
   ```

5. **Acceder a la aplicación**
   - La aplicación se ejecutará en: `https://localhost:5001` o `http://localhost:5000`
   - Navegar a la URL mostrada en la consola

### **Ejecutar Tests**

```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar con detalles verbose
dotnet test --verbosity normal

# Ejecutar con cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 🎯 Conceptos de Desarrollo Demostrados

### **1. Patrón MVC**
- **Separación de responsabilidades** entre Models, Views y Controllers
- **Routing** configurado para URLs amigables
- **Action Results** apropiados para diferentes tipos de respuesta

### **2. Inyección de Dependencias**
- **Registro de servicios** en el contenedor DI
- **Inversión de control** para mejor testabilidad
- **Interfaces** para abstracción de implementaciones

### **3. Testing Avanzado**
- **Integration Testing** con `WebApplicationFactory`
- **Unit Testing** con mocking usando Moq
- **Configuración de servicios** específica para tests

### **4. Frontend Responsivo**
- **Bootstrap 5** para diseño responsivo
- **Cards y Grid System** para layout moderno
- **Formularios validados** con HTML5 y CSS

### **5. Buenas Prácticas**
- **Separation of Concerns** (Separación de responsabilidades)
- **SOLID Principles** aplicados en servicios
- **Clean Code** con nomenclatura clara y comentarios
- **Error Handling** centralizado

## 📊 Estructura de Datos y Flujo

### **Flujo de la Aplicación**

1. **Inicio** → `HomeController.Index()` → Llama `SimpleService.HoraEsPar()` → Muestra resultado
2. **Login** → `HomeController.Login()` → Formulario → `ValidarLogin()` → Validación → Resultado
3. **Número Aleatorio** → `HomeController.NumeroAleatorio()` → `SimpleService.NumeroAleatorio()` → Resultado

### **ViewBag Usage**
- `ViewBag.ShowMessage`: Controla qué mensaje mostrar en Index
- `ViewBag.NumeroAleatorio`: Pasa el número generado a la vista
- `ViewBag.EsValido` y `ViewBag.Mensaje`: Manejo de resultados de login

## 🧪 Demo de Test Doubles - Solución Simplificada

### **¿Por qué esta solución es mejor?**
- ✅ **Un solo servicio** en lugar de múltiples interfaces complejas
- ✅ **Una sola acción** del controlador en lugar de flujo complejo
- ✅ **Vista simple** que muestra resultados directos
- ✅ **Tests claros** que demuestran cada concepto por separado
- ✅ **Fácil de entender** para estudiantes

### **¿Cómo funciona?**

#### **El Servicio `ITestDoublesService`**
```csharp
public interface ITestDoublesService
{
    string ProcesarTexto(ILogger logger, string texto);     // DUMMY
    decimal CalcularDescuento(string tipo);                // STUB  
    void GuardarDato(string key, string value);            // FAKE
    string ObtenerDato(string key);                        // FAKE
    void EnviarNotificacion(string mensaje);               // MOCK
    bool ValidarYProcesar(string input);                   // SPY
}
```

#### **El Controlador Simple**
```csharp
public IActionResult TestDoubles(string input = "demo")
{
    var resultado = new
    {
        TextoProcesado = _testDoublesService.ProcesarTexto(_logger, input),  // DUMMY
        DescuentoVIP = _testDoublesService.CalcularDescuento("VIP"),         // STUB
        DatoGuardado = GuardarYObtenerDato(input),                          // FAKE
        ValidacionExitosa = _testDoublesService.ValidarYProcesar(input)      // SPY
    };
    
    _testDoublesService.EnviarNotificacion($"Demo ejecutado: {input}");     // MOCK
    
    ViewBag.Resultado = resultado;
    return View();
}
```

#### **Los Tests Simples**
Cada tipo de Test Double tiene su propio test claro:

```csharp
[Fact] 
public void DUMMY_ProcesarTexto_LoggerNoSeUsa() 
{
    var dummyLogger = new Mock<ILogger>().Object;  // DUMMY - no verificamos
    var resultado = service.ProcesarTexto(dummyLogger, "hola");
    resultado.Should().Be("HOLA");
}

[Fact]
public void STUB_CalcularDescuento_RetornaValoresPredefinidos()
{
    var stubService = new Mock<ITestDoublesService>();
    stubService.Setup(x => x.CalcularDescuento("VIP")).Returns(0.20m);  // STUB
}

[Fact] 
public void MOCK_EnviarNotificacion_VerificaLlamada()
{
    var mockService = new Mock<ITestDoublesService>();
    mockService.Object.EnviarNotificacion("test");
    mockService.Verify(x => x.EnviarNotificacion("test"), Times.Once);  // MOCK
}
```

### **Ventajas Educativas**
- 🎯 **Cada concepto es claro** - Un método = Un tipo de Test Double
- 📚 **Progresión lógica** - De DUMMY (más simple) a SPY (más complejo)  
- 🔧 **Fácil de modificar** - Los estudiantes pueden experimentar
- 📖 **Tests legibles** - Cada test demuestra un solo concepto
- ⚡ **Rápido de entender** - No hay lógica de negocio compleja que distraiga

## 🔍 Detalles de Implementación Interesantes

### **SimpleService - Lógica de Negocio**
```csharp
public bool HoraEsPar()
{
    // Usa DateTime.Now.Hour % 2 para determinar si la hora es par
    return DateTime.Now.Hour % 2 == 0;
}

public bool ValidarUsuario(string usuario, string password)
{
    // Credenciales hardcodeadas para demostración
    return usuario == "don_correcto" && password == "iatusabes";
}
```

### **Testing con WebApplicationFactory**
- Permite **testing de integración** completo
- **Sustitución de servicios** para testing aislado
- **Simulación del pipeline HTTP** completo

## 📝 Posibles Mejoras y Extensiones

### **Posibles Mejoras y Extensiones**
- [ ] Autenticación real con JWT o Cookies
- [ ] Base de datos para persistencia
- [ ] API REST endpoints
- [ ] Logging estructurado con Serilog
- [ ] Validación de modelos con Data Annotations
- [ ] **🧪 Más Test Doubles**: Agregar ejemplos de combinaciones complejas
- [ ] **Tests parametrizados**: Usar Theory/InlineData para más casos
- [ ] **Performance tests**: Comparar rendimiento con/sin mocks

### **Testing**
- [ ] Tests de UI con Selenium
- [ ] Tests de carga con NBomber
- [ ] Cobertura de código al 100%
- [ ] Tests de mutación

### **Arquitectura**
- [ ] Clean Architecture con capas separadas
- [ ] CQRS con MediatR
- [ ] Repository Pattern
- [ ] Docker containerización

## 👥 Propósito Educativo

Este proyecto sirve como **ejemplo educativo** para:
- **Estudiantes** aprendiendo ASP.NET Core MVC
- **Desarrolladores** nuevos en testing con .NET
- **Demostraciones** de buenas prácticas de desarrollo
- **Base** para proyectos más complejos
- **🧪 Enseñanza de Test Doubles**: Material completo para clases de programación
- **Instructores**: Ejemplos listos para usar en cursos de testing
- **Práctica de TDD**: Casos reales para practicar Test Driven Development

## 📄 Licencia

Proyecto de demostración para propósitos educativos.

---

*Desarrollado como ejemplo de aplicación ASP.NET Core MVC con testing completo y buenas prácticas de desarrollo.*
