using System;
using System.Collections.Generic;

class Blackjack
{
    static void Main()
    {
        JuegoBlackjack juego = new JuegoBlackjack();
        juego.IniciarJuego();
    }
}

class Carta
{
    public string Palo { get; }
    public string Valor { get; }
    public int Puntos { get; }

    public Carta(string palo, string valor, int puntos)
    {
        Palo = palo;
        Valor = valor;
        Puntos = puntos;
    }

    public override string ToString()
    {
        return $"{Valor} de {Palo}";
    }
}

class JuegoBlackjack
{
    private List<Carta> mazo;
    private List<Carta> manoJugador;
    private List<Carta> manoBanca;
    private Random random;

    public JuegoBlackjack()
    {
        mazo = new List<Carta>();
        manoJugador = new List<Carta>();
        manoBanca = new List<Carta>();
        random = new Random();
        InicializarMazo();
    }

    private void InicializarMazo()
    {
        string[] palos = { "Corazones", "Tréboles", "Picas", "Diamantes" };
        string[] figuras = { "J", "Q", "K" };

        foreach (string palo in palos)
        {
            for (int i = 1; i <= 10; i++)
            {
                mazo.Add(new Carta(palo, i.ToString(), i));
            }
            foreach (string figura in figuras)
            {
                mazo.Add(new Carta(palo, figura, 10));
            }
        }
    }

    public void IniciarJuego()
    {
        Console.WriteLine("Bienvenido a Blackjack!");
        RepartirCarta(manoJugador);
        RepartirCarta(manoJugador);
        MostrarMano(manoJugador, "Tu mano");

        while (true)
        {
            Console.Write("¿Quieres otra carta? (s/n): ");
            string respuesta = Console.ReadLine().ToLower();
            
            if (respuesta == "s")
            {
                RepartirCarta(manoJugador);
                MostrarMano(manoJugador, "Tu mano");
                if (CalcularPuntaje(manoJugador) > 21)
                {
                    Console.WriteLine("¡Te has pasado! Has perdido.");
                    return;
                }
            }
            else if (respuesta == "n")
            {
                Console.WriteLine("Te has plantado. Ahora juega la banca.");
                TurnoBanca();
                return;
            }
        }
    }

    private void TurnoBanca()
    {
        while (CalcularPuntaje(manoBanca) < 17)
        {
            RepartirCarta(manoBanca);
        }
        MostrarMano(manoBanca, "Mano de la banca");
        DeterminarGanador();
    }

    private void RepartirCarta(List<Carta> mano)
    {
        int indice = random.Next(mazo.Count);
        Carta carta = mazo[indice];
        mazo.RemoveAt(indice);
        mano.Add(carta);
    }

    private void MostrarMano(List<Carta> mano, string titulo)
    {
        Console.WriteLine(titulo + ":");
        foreach (Carta carta in mano)
        {
            Console.WriteLine(carta);
        }
        Console.WriteLine("Puntaje actual: " + CalcularPuntaje(mano));
    }

    private int CalcularPuntaje(List<Carta> mano)
    {
        int suma = 0;
        int ases = 0;
        foreach (Carta carta in mano)
        {
            suma += carta.Puntos;
            if (carta.Valor == "1") ases++;
        }
        while (ases > 0 && suma + 10 <= 21)
        {
            suma += 10;
            ases--;
        }
        return suma;
    }

    private void DeterminarGanador()
    {
        int puntajeJugador = CalcularPuntaje(manoJugador);
        int puntajeBanca = CalcularPuntaje(manoBanca);
        
        Console.WriteLine($"Puntaje final - Jugador: {puntajeJugador}, Banca: {puntajeBanca}");
        
        if (puntajeJugador > 21)
        {
            Console.WriteLine("Has perdido, te pasaste de 21.");
        }
        else if (puntajeBanca > 21 || puntajeJugador > puntajeBanca)
        {
            Console.WriteLine("¡Felicidades! Has ganado.");
        }
        else if (puntajeJugador < puntajeBanca)
        {
            Console.WriteLine("La banca gana. Mejor suerte la próxima vez.");
        }
        else
        {
            Console.WriteLine("Empate.");
        }
    }
}
