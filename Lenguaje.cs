using System;
using System.Collections.Generic;
/*Requerimiento 1.- Eliminar las dobles comillas del printf e interpretar las secuencias
                    dentro de la cadena
  Requerimiento 2.- Marcar los errores sintancticos cuando la variable no exista
                    -si la variable no existe declarar que no existe
  Requerimiento 3.- Modificar el valor de la variable en la asignacion en el metodo de
                    asiganacion.
  Requerimiento 4.- Obtener el valor de la variable cuando se requiera y programar el metodo
                    getValor.
  Requerimiento 5.- Modificar el valor de la variable en el Scanf.
*/
namespace Evalua
{

    public class Lenguaje : Sintaxis
    {
        //MINUSCULA PARA EVITAR QUE EXISTAN DOS Variables CON EL METODO VARIABLES
        //NO SE PUEDE NOMBRAR AL IGUAL QUE UN METODO PREVIAMENTE PROGRAMADO 
        List<Variable> variables = new List<Variable>();
        Stack<float> stack = new Stack<float>();
        public Lenguaje()
        {

        }
        public Lenguaje(string nombre) : base(nombre)
        {

        }
        //PASAMOS EL NOMBRE Y TIPO DE DATO Y QUE LO AGREGUE A LA LISTA
        private void addVariable(string nombre, Variable.TipoDato tipo)
        {
            //AGREGAMOS A LA LISTA UNA NUEVA VARIABLE
            variables.Add(new Variable(nombre, tipo));
        }
        private void displayVariables( )
        {
            log.WriteLine();
            log.WriteLine("Variables: ");
            foreach (Variable v in variables)
            {
                log.WriteLine(v.getNombre() + " " + v.getTipoDato() + " " + v.getValor());
            }
        }

        private bool existeVariable (string nombre) //SABER SI EXISTE LA VARIABLE
        {
            foreach (Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                    return true;
            }
            return false;
        }

        //modificavariable
        private void modVariable (string nombre, float nuevoValor)
        {
            //SE GENERA CON UN FOREACH 
            foreach (Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    v.setValor(nuevoValor);
                }
            }
        }

        private float getValor (string nombreVariable)
        {
            //PARA BUSCAR VARIABLE
            foreach (Variable v in variables)
            {
                if (v.getNombre() == nombreVariable)
                {
                    
                }
            }
            //EN CASO DE NO ENCONTRAR LA VARIABLE
            /*if (!existe)
            {
                throw new Error("\nError la variable <" + getContenido() +"> en linea: "+linea, log);
            }*/
            return 0;
        }
        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            Libreria();
            Variables();
            Main();
            displayVariables();
        }

        //Librerias -> #include<identificador(.h)?> Librerias?
        private void Libreria()
        {
            if (getContenido() == "#")
            {
                match("#");
                match("include");
                match("<");
                match(Tipos.Identificador);
                if (getContenido() == ".")
                {
                    match(".");
                    match("h");
                }
                match(">");
                Libreria();
            }
        }

         //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variable.TipoDato tipo = Variable.TipoDato.Char;
                switch (getContenido())
                {
                    case "int": tipo = Variable.TipoDato.Int; break;
                    case "float": tipo = Variable.TipoDato.Float; break;
                }
                match(Tipos.TipoDato);
                Lista_identificadores(tipo);
                match(Tipos.FinSentencia);
                Variables();
            }
        }

        //Lista_identificadores -> identificador (,Lista_identificadores)?
        private void Lista_identificadores(Variable.TipoDato tipo)      //RECIBE EL TIPO DE DATO DE LA VARIABLE
        {
            if (getClasificacion() == Tipos.Identificador)
            {
                if (!existeVariable(getContenido()))
                {
                    addVariable(getContenido(), tipo);
                }
                else
                {
                    throw new Error("Error de sintaxis, variable duplicada <" + getContenido() +"> en linea: "+linea, log);
                }
            }
            match(Tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                Lista_identificadores(tipo);
            }
        }
        //Bloque de instrucciones -> {listaIntrucciones?}
        private void BloqueInstrucciones()
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones();
            }    
            match("}"); 
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones()
        {
            Instruccion();
            if (getContenido() != "}")
            {
                ListaInstrucciones();
            }
        }

        //ListaInstruccionesCase -> Instruccion ListaInstruccionesCase?
        private void ListaInstruccionesCase()
        {
            Instruccion();
            if (getContenido() != "case" && getContenido() !=  "break" && getContenido() != "default" && getContenido() != "}")
            {
                ListaInstruccionesCase();
            }
        }

        //Instruccion -> Printf | Scanf | If | While | do while | For | Switch | Asignacion
        private void Instruccion()
        {
            if (getContenido() == "printf")
            {
                Printf();
            }
            else if (getContenido() == "scanf")
            {
                Scanf();
            }
            else if (getContenido() == "if")
            {
                If();
            }
            else if (getContenido() == "while")
            {
                While();
            }
            else if(getContenido() == "do")
            {
                Do();
            }
            else if(getContenido() == "for")
            {
                For();
            }
            else if(getContenido() == "switch")
            {
                Switch();
            }
            else
            {
                Asignacion();
            }
        }

        //Asignacion -> identificador = cadena | Expresion;
        private void Asignacion()
        {
            //REQUERIMIENTO 2-(SI NO EXISTE LA VARIABLE(GETCONTENIDO) 
            //SE LEVANTA LA EXCEPCION) Y TERMINA EL PROGRAMA
            log.WriteLine();
            log.Write(getContenido()+" = ");
            string nombreVariable = getContenido();
            bool existe = existeVariable(nombreVariable);
            if (!existe)
            {
                throw new Error("\nError la variable <" + getContenido() +"> en linea: "+linea, log);
            }
            match(Tipos.Identificador);             
            //DEBE DE EXISTIR LA VARIABLE SI NO SE LEVNATA LA EXCEPCION
            match(Tipos.Asignacion);
            //SE ELIMINO EL IF PORQUE NO EXISTE EL TIPO DE DATO CADENA
            Expresion();
            match(";");
            //GUARDAR EL RESULTADO
            float resultado = stack.Pop();
            log.Write("= " + resultado);
            log.WriteLine();
            //AQUI SE MODIFICA EL RESULTADO A LA VARIABLE
            modVariable(nombreVariable, resultado);
        }

        //While -> while(Condicion) bloque de instrucciones | instruccion
        private void While()
        {
            match("while");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{") 
            {
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }

        //Do -> do bloque de instrucciones | intruccion while(Condicion)
        private void Do()
        {
            match("do");
            if (getContenido() == "{")
            {
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            } 
            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Intruccion 
        private void For()
        {
            match("for");
            match("(");
            Asignacion();
            Condicion();
            match(";");
            Incremento();
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones();  
            }
            else
            {
                Instruccion();
            }
        }

        //Incremento -> Identificador ++ | --
        private void Incremento()
        {
            // REQUERIMIENTO 2 SI NO EXISTE LA VARIABLE SE LEVANTA LA EXCEPCION
            //GUARDAR EL VALOR DE LA VARIABELS
            string variable = getContenido();
            //string nombreVariable = getContenido();
            bool existe = existeVariable(variable);
            if (!existe)
            {
                throw new Error("\nError la variable <" + getContenido() +"> en linea: "+linea, log);
            }
            match(Tipos.Identificador);
            if(getContenido() == "++")
            {
                //REQUERIMIENTO 4 - OBTENER EL VALOS DE LA VARIABLE INCREMENTAR UNO 
                //Y VOLVER A METER EL NUEVO VALOR
                match("++");
                //                    LEO EL VALOR DE LA VARIABLE Y LE SUMO 1  
                modVariable(variable, getValor(variable)+1);
            }
            else
            {
                //LO MISMO DE ARRIBA PERO CON -1
                match("--");
                modVariable(variable, getValor(variable)-1);
            }
        }

        //Switch -> switch (Expresion) {Lista de casos} | (default: )
        private void Switch()
        {
            match("switch");
            match("(");
            Expresion();
            stack.Pop(); //NO SE LE ASIGAN A NADA Y SE PIERDE EL VALOR
            match(")");
            match("{");
            ListaDeCasos();
            if(getContenido() == "default")
            {
                match("default");
                match(":");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones();  
                }
                else
                {
                    Instruccion();
                }
            }
            match("}");
        }

        //ListaDeCasos -> case Expresion: listaInstruccionesCase (break;)? (ListaDeCasos)?
        private void ListaDeCasos()
        {
            match("case");
            Expresion();
            stack.Pop();
            match(":");
            ListaInstruccionesCase();
            if(getContenido() == "break")
            {
                match("break");
                match(";");
            }
            if(getContenido() == "case")
            {
                ListaDeCasos();
            }
        }

        //Condicion -> Expresion operador relacional Expresion
        private void Condicion()
        {
            Expresion();
            stack.Pop();
            match(Tipos.OperadorRelacional);
            Expresion();
        }

        //If -> if(Condicion) bloque de instrucciones (else bloque de instrucciones)?
        private void If()
        {
            match("if");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones();  
            }
            else
            {
                Instruccion();
            }
            if (getContenido() == "else")
            {
                match("else");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones();
                }
                else
                {
                    Instruccion();
                }
            }
        }

        //Printf -> printf(cadena o expreción);
        private void Printf()
        {
            match("printf");
            match("(");
            if (getClasificacion() == Tipos.Cadena)
            {
                //REQUERIMIENTO 1 ELIMINAR COMILLAS DOBLES
                /*String.Replace(x, y) se utiliza para reemplazar
                todas las apariciones de la cadena x con la cadena y */
                setContenido(getContenido().Replace("\"", ""));
                setContenido(getContenido().Replace("\\n","\n"));
                setContenido(getContenido().Replace("\\t","\t"));
                Console.Write(getContenido());
                match(Tipos.Cadena);
            }
            //SI NO ES CADENA ES EXPRECIÓN
            else
            {
                Expresion();
                Console.Write(stack.Pop());
            }
            match(")");
            match(";");
        }

        //Scanf -> scanf(cadena,&identificador);
        private void Scanf()    
        {
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");
            //REQUERIMIENTO 2
            string nombreVariable = getContenido();
            bool existe = existeVariable(nombreVariable);
            if (!existe)
            {
                throw new Error("\nError la variable <" + getContenido() +"> en linea: "+linea, log);
            }
            string val = ""+Console.ReadLine();
            //REQUERIMIENTO 5
            //YA SE CAPTURO EL STRING DEL VALOR HAY QUE CONVERTIR A FLOAT
            match(Tipos.Identificador);
            match(")");
            match(";");
        }

        //Main      -> void main() Bloque de instrucciones
        private void Main()
        {
            match("void");
            match("main");
            match("(");
            match(")");
            BloqueInstrucciones();
        }

        //Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino();
                log.Write(operador + " ");
                //SACAMOS NUMEROS DEL STACK
                float n1 = stack.Pop();
                float n2 = stack.Pop();
                switch (operador)
                {
                    case "+":
                        stack.Push(n2 + n1);
                        break;
                    case "-":
                        stack.Push(n2 - n1);
                        break;
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        //PorFactor -> (OperadorFactor Factor)? 
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor();
                log.Write(operador + " ");
                float n1 = stack.Pop();
                float n2 = stack.Pop();
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        break;
                    case "/":
                        stack.Push(n2 / n1);
                        break;
                }
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasificacion() == Tipos.Numero)
            {
                log.Write(getContenido() + " ");
                stack.Push(float.Parse(getContenido())); //POP PARA METER NUMERO
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                // REQUERIMIENTO 2 SI NO EXISTE LA VARIABLE SE LEVANTA LA EXCEPCION
                string nombreVariable = getContenido();
                bool existe = existeVariable(nombreVariable);
                if (!existe)
                {
                    throw new Error("\nError la variable <" + getContenido() +"> en linea: "+linea, log);
                }
                log.Write(getContenido() + " ");
                //BUSCA EL CONTENIDO Y LO BUSCA
                stack.Push(getValor(getContenido())); //POP PARA METER NUMERO
                match(Tipos.Identificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }
    }
}