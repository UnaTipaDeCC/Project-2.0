# Project-2.0
Funcionamiento del Juego
Al iniciar el juego, se presenta un menú inicial con dos opciones:
Jugar: Esta opción lleva al jugador a la escena del juego.
Create Card: Esta opción abre la escena donde se puede ingresar el código necesario para crear cartas, tras su proceso de parseo, chequeo semántico y evaluación.
Componentes del Juego
Cada jugador cuenta con un mazo que contiene las siguientes cartas:
Cartas de Unidad: Pueden ser de plata u oro. Estas cartas no son afectadas por las cartas especiales y pueden tener efectos que se activan una sola vez al ser jugadas.
Cartas Especiales:
Clima: Afectan a las cartas de plata de una zona específica de ambos jugadores que estén en el campo en el momento en que se juegan.
Despeje: Envía al cementerio las cartas de clima.
Aumento: Afectan a una fila específica y solo a las cartas que estén en el campo en el momento en que se juega.
Mecánica del Juego
Una vez en la escena del juego, cada jugador recibe 10 cartas en su mano, con la opción de hacer clic derecho en dos de ellas para cambiarlas por otras aleatorias de su mazo. Una vez que una carta es jugada, el jugador pierde la oportunidad de cambiarla.
Turnos: El primer turno corresponde al jugador de la facción Hormigas Bravas (Jugador Uno).
Acciones por Turno: Cada turno consiste en jugar una carta de la mano, invocar la carta líder o pasar.
Fin de Ronda: La ronda termina cuando ambos jugadores se han pasado. Se comparan los contadores de ambos jugadores y se actualiza el contador de partidas ganadas:
El ganador recibe un punto.
En caso de empate, ambos jugadores reciben un punto.
Final del Juego:
El juego culmina cuando uno de los jugadores ha ganado al menos dos partidas.

Aspectos Generales de la Estructura del Código
Los scripts están organizados en tres carpetas principales:
1. GameScripts
Contiene todo lo relacionado con el manejo del juego:
GameManager: Controla aspectos generales del juego, como el final de las rondas y del juego, el control de los turnos y la restauración de todos los valores al inicio de los mismos.
Player: Contiene referencias a cada una de las zonas del jugador, incluyendo la zona de la carta líder (que se instancia al iniciar el juego). Además incluye:
Métodos y propiedades como field(lista de todas las cartas en el campo), Pop, SendBottom
Control del robo y cambio de cartas.
Puntos ganados en la ronda y rondas ganadas.
GameContext: Contiene métodos y propiedades accesibles según las instrucciones (Pop, Hand, Remove, etc.), que son vitales para la ejecución de los efectos creados por el usuario.
Zones: Contiene la lista de cartas de cada zona y el método refreshZone para instanciar y destruir cartas según sea necesario.
2. CardScripts
Contiene todo lo relacionado con el control de las cartas, incluyendo:
CardMove: Maneja el movimiento de las cartas.
Card: Contiene la información de las cartas.
Effect: Gestiona los efectos de las cartas.
3. Interprete
Contiene todo lo relacionado con el proceso de tokenización, parseo, chequeo semántico y evaluación de las cartas y sus componentes. Está estructurado en un AST (Árbol de Sintaxis Abstracta) que maneja:
-Expresiones en todas sus variantes.
-Declaraciones relacionadas con las cartas y sus efectos.
El analisis lexico(lexer), constituye la primera parte del proceso del interprete y consiste en tomar el texto de entrada, analizarlo caracter por caracter y convertirlo en una lista de tokens, posteriormente el Parser toma estos tokens y los organiza en estas estructuras que forman parte del AST, tambien se asegura de que sigan el orden y la estructura correcta. Una vez terminado el proceso de parseo, si no ocurren errores, se realiza el chequeo semantico (cada nodo del AST realiza su propio chequeo semántico), se procede a la evaluacion de las cartas(y por ende todos sus componentes), en el transcurso del juego, segun sea necesario, se evaluan los efectos. (Cada nodo del arbol contiene su propio metodo evaluate.)
Asimismo, este proyecto contiene el Scope que controla lo relacionado a las variables y sus entornos, Context que maneja y almacena informacion de las propiedades y metodos permitidos, los efectos declarados, entre otros.
Cada una de estas carpetas contiene otros scripts que controlan aspectos mas generales.
Respecto al DSL a original, este proyecto contiene un par de modificaciones como son la exigencia de una coma tras la declaracion del "Predicate", cada "Selector" y cada "PostAction".