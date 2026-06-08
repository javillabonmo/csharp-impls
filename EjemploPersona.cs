using System;
using System.Collections.Generic;

namespace EjemploPersona
{
    /// <summary>
    /// Representa una persona con atributos, comportamientos y otros miembros.
    /// </summary>
    public class Persona : IComparable<Persona>, ICloneable
    {
        #region Campos y Constantes

        /// <summary>
        /// Constante de ejemplo, que podría representar el número máximo de intentos en alguna operación.
        /// </summary>
        public const int MAX_INTENTOS = 3;

        /// <summary>
        /// Contador de instancias creadas de la clase Persona.
        /// </summary>
        private static int _contadorInstancias = 0;

        /// <summary>
        /// Almacena el nombre de la persona.
        /// </summary>
        private string _nombre;

        /// <summary>
        /// Almacena el apellido de la persona.
        /// </summary>
        private string _apellido;

        /// <summary>
        /// Almacena la edad de la persona.
        /// </summary>
        private int _edad;

        /// <summary>
        /// Lista de direcciones asociadas a la persona.
        /// </summary>
        private List<string> _direcciones;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene o establece el nombre completo (nombre y apellido separados por un espacio).
        /// </summary>
        public string NombreCompleto
        {
            get { return $"{_nombre} {_apellido}"; }
            set
            {
                var partes = value.Split(' ');
                if (partes.Length >= 2)
                {
                    _nombre = partes[0];
                    _apellido = partes[1];
                }
                else
                {
                    throw new ArgumentException("Debe proporcionar nombre y apellido.");
                }
            }
        }

        /// <summary>
        /// Obtiene o establece la edad, validando que no sea negativa.
        /// </summary>
        public int Edad
        {
            get { return _edad; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("La edad no puede ser negativa.");
                _edad = value;
                // Dispara el evento cuando la edad cambia.
                OnEdadCambiada(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Propiedad estática solo de lectura que devuelve el número total de instancias de Persona.
        /// </summary>
        public static int ContadorInstancias
        {
            get { return _contadorInstancias; }
        }

        /// <summary>
        /// Proporciona una vista de solo lectura de las direcciones de la persona.
        /// </summary>
        public IReadOnlyList<string> Direcciones
        {
            get { return _direcciones.AsReadOnly(); }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor estático para inicializar miembros estáticos.
        /// </summary>
        static Persona()
        {
            _contadorInstancias = 0;
        }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public Persona()
        {
            _nombre = "Nombre";
            _apellido = "Apellido";
            _edad = 0;
            _direcciones = new List<string>();
            _contadorInstancias++;
        }

        /// <summary>
        /// Constructor sobrecargado que recibe nombre, apellido y edad.
        /// </summary>
        /// <param name="nombre">Primer nombre de la persona.</param>
        /// <param name="apellido">Apellido de la persona.</param>
        /// <param name="edad">Edad de la persona.</param>
        public Persona(string nombre, string apellido, int edad)
        {
            _nombre = nombre;
            _apellido = apellido;
            Edad = edad; // Se utiliza la propiedad para aprovechar la validación y disparo de evento.
            _direcciones = new List<string>();
            _contadorInstancias++;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se dispara cuando la edad de la persona cambia.
        /// </summary>
        public event EventHandler EdadCambiada;

        /// <summary>
        /// Método protegido que dispara el evento de cambio de edad.
        /// </summary>
        /// <param name="e">Argumentos del evento.</param>
        protected virtual void OnEdadCambiada(EventArgs e)
        {
            EdadCambiada?.Invoke(this, e);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Agrega una nueva dirección a la lista de direcciones.
        /// </summary>
        /// <param name="direccion">La dirección a agregar.</param>
        public void AgregarDireccion(string direccion)
        {
            if (string.IsNullOrWhiteSpace(direccion))
                throw new ArgumentException("La dirección no puede ser vacía.");
            _direcciones.Add(direccion);
        }

        /// <summary>
        /// Remueve una dirección de la lista.
        /// </summary>
        /// <param name="direccion">La dirección a remover.</param>
        /// <returns>Devuelve true si la dirección fue removida exitosamente.</returns>
        public bool RemoverDireccion(string direccion)
        {
            return _direcciones.Remove(direccion);
        }

        /// <summary>
        /// Imprime en consola la información de la persona (nombre completo, edad y direcciones).
        /// </summary>
        public void MostrarInformacion()
        {
            Console.WriteLine("Información de la persona:");
            Console.WriteLine($"Nombre completo: {NombreCompleto}");
            Console.WriteLine($"Edad: {Edad}");
            Console.WriteLine("Direcciones:");
            foreach (var dir in _direcciones)
            {
                Console.WriteLine($" - {dir}");
            }
        }

        /// <summary>
        /// Método estático para mostrar el total de instancias creadas.
        /// </summary>
        public static void MostrarContadorInstancias()
        {
            Console.WriteLine($"Total de instancias de Persona: {ContadorInstancias}");
        }

        /// <summary>
        /// Implementa la comparación de dos personas según su edad.
        /// </summary>
        /// <param name="other">Otra instancia de Persona para comparar.</param>
        /// <returns>
        /// Un valor menor que cero si esta persona es menor,
        /// cero si ambas tienen la misma edad o mayor que cero si es mayor.
        /// </returns>
        public int CompareTo(Persona other)
        {
            if (other == null)
                return 1;
            return this.Edad.CompareTo(other.Edad);
        }

        /// <summary>
        /// Realiza una copia superficial de este objeto Persona.
        /// </summary>
        /// <returns>Un nuevo objeto clonado de tipo Persona.</returns>
        public object Clone()
        {
            // Se utiliza MemberwiseClone para copiado superficial... 
            Persona clon = (Persona)this.MemberwiseClone();
            // Se crea una nueva lista para evitar que el clon comparta la misma referencia de direcciones.
            clon._direcciones = new List<string>(this._direcciones);
            return clon;
        }

        /// <summary>
        /// Sobrescribe el método ToString para devolver la información básica de la persona.
        /// </summary>
        /// <returns>Una cadena representativa de la persona.</returns>
        public override string ToString()
        {
            return $"Persona: {NombreCompleto}, Edad: {Edad}";
        }

        #endregion

        #region Indexador

        /// <summary>
        /// Permite acceder o modificar las direcciones a través de un índice.
        /// </summary>
        /// <param name="index">Índice de la dirección.</param>
        /// <returns>La dirección contenida en el índice indicado.</returns>
        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= _direcciones.Count)
                    throw new IndexOutOfRangeException("Índice fuera de rango.");
                return _direcciones[index];
            }
            set
            {
                if (index < 0 || index >= _direcciones.Count)
                    throw new IndexOutOfRangeException("Índice fuera de rango.");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La dirección no puede ser vacía.");
                _direcciones[index] = value;
            }
        }

        #endregion

        #region Tipos Anidados

        /// <summary>
        /// Enumeración que define los tipos de documento posibles.
        /// </summary>
        public enum TipoDocumento
        {
            DNI,
            Pasaporte,
            LicenciaConducir
        }

        /// <summary>
        /// Clase anidada que representa un documento asociado a una persona.
        /// </summary>
        public class Documento
        {
            /// <summary>
            /// Número del documento.
            /// </summary>
            public string Numero { get; set; }
            /// <summary>
            /// Tipo de documento.
            /// </summary>
            public TipoDocumento Tipo { get; set; }

            /// <summary>
            /// Constructor del documento.
            /// </summary>
            /// <param name="numero">Número del documento.</param>
            /// <param name="tipo">Tipo del documento.</param>
            public Documento(string numero, TipoDocumento tipo)
            {
                Numero = numero;
                Tipo = tipo;
            }

            /// <summary>
            /// Sobrescribe el método ToString para mostrar la información del documento.
            /// </summary>
            /// <returns>Cadena representativa del documento.</returns>
            public override string ToString()
            {
                return $"{Tipo}: {Numero}";
            }
        }

        #endregion

        #region Destructor / Finalizador

        /// <summary>
        /// Finalizador de la clase Persona que se invoca cuando el recolector de basura libera la instancia de memoria.
        /// </summary>
        ~Persona()
        {
            Console.WriteLine($"El objeto Persona '{NombreCompleto}' está siendo recolectado.");
        }

        #endregion
    }

    /// <summary>
    /// Clase Program para demostrar el uso de la clase Persona.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Creación de instancias de Persona
            Persona persona1 = new Persona("Juan", "Pérez", 30);
            Persona persona2 = new Persona("Ana", "García", 25);

            // Suscribirse al evento de cambio de edad de persona1
            persona1.EdadCambiada += (sender, e) =>
            {
                Console.WriteLine("La edad de la persona ha cambiado.");
            };

            // Agregar direcciones a persona1
            persona1.AgregarDireccion("Calle 123");
            persona1.AgregarDireccion("Avenida 456");

            // Modificar la edad, lo que disparará el evento
            persona1.Edad = 31;

            // Mostrar información de persona1
            persona1.MostrarInformacion();

            // Uso del indexador para acceder a direcciones
            Console.WriteLine($"Primera dirección: {persona1[0]}");

            // Comparar las edades de persona1 y persona2
            int comparacion = persona1.CompareTo(persona2);
            if (comparacion > 0)
            {
                Console.WriteLine("persona1 es mayor que persona2");
            }
            else if (comparacion < 0)
            {
                Console.WriteLine("persona1 es menor que persona2");
            }
            else
            {
                Console.WriteLine("Ambas personas tienen la misma edad");
            }

            // Clonación de persona1
            Persona clonPersona1 = (Persona)persona1.Clone();
            Console.WriteLine("Información del clon de persona1:");
            clonPersona1.MostrarInformacion();

            // Mostrar el total de instancias creadas
            Persona.MostrarContadorInstancias();

            // Esperar para visualizar la salida
            Console.WriteLine("Presiona cualquier tecla para finalizar...");
            Console.ReadKey();
        }
    }
}