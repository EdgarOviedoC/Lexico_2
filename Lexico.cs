using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Lexico_2
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        StreamWriter asm;
        int line = 1;

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

        public void nexToken()
        {
            char c;
            string Buffer = "";

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

        Numero -> 
        
        FinSentencia -> 
        InicioBloque -> 
        FinBloque -> 
        OperadorTernario -> 
        OperadorTermino -> 
        OperadorFactor -> 
        IncrementoTermino -> 
        IncrementoFactor -> 
        OperadorRelacional -> 
        OperadorLogico -> 
        Puntero -> 
        Asignacion -> 
        Moneda -> 
        Cadena -> 

    Automata: Modelo matematico que representa una expresion regular a travez de 
    un GRAFO, para una maquina de estado finito que consiste en un conjunto de 
    estados bien definidos, un estado inicial, un alfabeto de entrada y una funcion 
    de transición
*/