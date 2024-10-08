using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Lexico_2
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        StreamWriter asm;
        int line = 1;
        const int F = -1;
        const int E = -2;

        //Constructor de la clase lexico
        public Lexico()
        {
            log = new StreamWriter("prueba.log");
            asm = new StreamWriter("prueba.asm");
            log.AutoFlush = true;
            asm.AutoFlush = true;

            if (File.Exists("prueba.cpp"))
            {
                archivo = new StreamReader("prueba.cpp");
            }
            else
            {
                throw new Error("El archivo pueba.cpp no existe", log);
            }
        }

        //Constructor sobrecargado
        public Lexico(string nombreArchivo)
        {
            log = new StreamWriter(Path.ChangeExtension(nombreArchivo, ".log"));
            log.AutoFlush = true;

            if (File.Exists(Path.ChangeExtension(nombreArchivo, ".cpp")))
            {
                archivo = new StreamReader(nombreArchivo);
            }
            else
            {
                throw new Error("El archivo " + nombreArchivo + " no existe", log);
            }

            if (Path.GetExtension(nombreArchivo) == ".cpp")
            {
                asm = new StreamWriter(Path.ChangeExtension(nombreArchivo, ".asm"));
                asm.AutoFlush = true;
            }
            else
            {
                throw new Error("El archivo tiene extension invalida", log);
            }

        }

        //Destructor de la clase lexico
        public void Dispose()
        {
            log.WriteLine("Total de lineas {0}", line);

            log.Close();
            archivo.Close();
            asm.Close();
        }
        private int automata(char c, int estado)
        {
            int nuevoEstado = estado;

            switch (estado)
            {
                case 0:
                    if (char.IsWhiteSpace(c))
                    {
                        nuevoEstado = 0;
                    }
                    else if (char.IsLetter(c))
                    {
                        nuevoEstado = 1;
                    }
                    else if (char.IsDigit(c))
                    {
                        nuevoEstado = 2;
                    }
                    else
                    {
                        nuevoEstado = 8;
                    }
                    break;
                case 1:
                    setClasificacion(Tipos.Identificador);
                    if (!(char.IsLetterOrDigit(c)))
                    {
                        nuevoEstado = F;
                    }
                    break;
                case 2:
                    setClasificacion(Tipos.Numero);
                    if (char.IsDigit(c))
                    {
                        nuevoEstado = 2;
                    }
                    else if (c == '.')
                    {
                        nuevoEstado = 3;
                    }
                    else if (char.ToLower(c) == 'e')
                    {
                        nuevoEstado = 5;
                    }
                    else
                    {
                        nuevoEstado = F;
                    }
                    break;
                case 3:
                    if (char.IsDigit(c))
                    {
                        nuevoEstado = 4;
                    }
                    else
                    {
                        nuevoEstado = E;
                    }
                    break;
                case 4:
                    if (char.IsDigit(c))
                    {
                        nuevoEstado = 4;
                    }
                    else if (char.ToLower(c) == 'e')
                    {
                        nuevoEstado = 5;
                    }
                    else
                    {
                        nuevoEstado = F;
                    }
                    break;
                case 5:
                    if (c == '+' || c == '-')
                    {
                        nuevoEstado = 6;
                    }
                    else if (char.IsDigit(c))
                    {
                        nuevoEstado = 7;
                    }
                    else
                    {
                        nuevoEstado = E;
                    }
                    break;
                case 6:
                    if (char.IsDigit(c))
                    {
                        nuevoEstado = 7;
                    }
                    else
                    {
                        nuevoEstado = E;
                    }
                    break;
                case 7:
                    nuevoEstado = 7;
                    if (!(char.IsDigit(c)))
                    {
                        nuevoEstado = F;
                    }
                    break;
                case 8:
                    nuevoEstado = F;
                    break;
            }
            return nuevoEstado;
        }
        public void nexToken()
        {
            char transicion;
            string Buffer = "";
            int estado = 0;

            while (estado > 0)
            {
                transicion = (char)archivo.Peek();
                estado = automata(transicion, estado);
                if (estado == E)
                {
                    if (getClasificacion() == Tipos.Numero)
                    {
                        throw new Error("Lexico, se espera un digito", log, line);
                    }
                }
                if (estado >= 0)
                {
                    archivo.Read();
                    if (transicion == '\n')
                    {
                        line++;
                    }
                    if (estado > 0)
                    {
                        Buffer += transicion;
                    }
                }

            }

            if (!finArchivo())
            {
                setContenido(Buffer);
                log.WriteLine("{0}  °°°°  {1}", getContenido(), getClasificacion());
            }
        }

        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}
/*
    Expresion Regular: Metodo formal que através de una secuencia de caracteres 
    que define un PATRON de busqueda
    
        a) Reglas BNF
        b) Reglas BNF extendidas
        c) Operaciones aplicadas al lenguaje

    OAL

        1. Concatenación simple (·)
        2. Concatenación exponencial (^)
        3. Cerradura de Kleene (*)
        4. Cerradura positiva (+)
        5. Cerradura Epsilon (?)
        6. Operador OR (|)
        7. Parentesis()

        L = {A, B, C, D, E, ..., Z, a, b, c, d, e, ..., z}
        D = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}

        1. L·D
            LD

        2. L^3 = LLL
           L^3D^2 = LLLDD

           D^5 = DDDDD
           =^2 = ==

        3. L* = Cero o más letras
           D* = Cero o más digítos

        4. L+ = Una o mas letras
           D+ = Uno o más digitos 

        5. L? = Cero o una letra (La letra es optativa-opcional)

        6. L | D = Letra o digito
           + | - = + ó - 

        7. (L D) L? = (Letra seguido de Digito y al final letra opcional)


    Producción Gramatical

        Clasificación del Token -> Expresión regular.

        Identificador -> L (L | D)* 

        Numero -> D+ (.D+)? (E(+ | -)? D+)?
        
        FinSentencia -> ;
        InicioBloque -> {
        FinBloque -> }
        OperadorTernario -> ?

        Puntero -> ->

        OperadorTermino -> + | -
        IncrementoTermino -> + (+ | =) | - (- | =)

        Termino + -> + (+ | =)?
        Termino - -> - (- | = | >)?

        OperadorFactor -> * | / | %
        IncrementoFactor -> *= | /= | %=

        Factor -> * | / | % (=)?
 
        OperadorLogico -> && | || | !

        NotOpRel -> ! (=)?
        
        Asignacion -> =

        AsigOpRel -> = (=)?

        OperadorRelacional -> > (=) ? | < (> | =)? | == | !=

        Cadena -> "c*"
        Caracter -> 'c' | #D* | lamda

    Automata: Modelo matematico que representa una expresion regular a travez de 
    un GRAFO, para una maquina de estado finito que consiste en un conjunto de 
    estados bien definidos: 
        - Un estado inicial
        - Un alfabeto de entrada
        - Una funcion de transición
*/