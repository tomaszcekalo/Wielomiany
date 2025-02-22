﻿using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;

// zalozenia

// tu wrzuce listy wszystkich parametrów aby jednym runem wykonac cale statystyki


// funkcje

static List<List<Double>> openFile(string path)
{
    List<List<Double>> wielomiany = new List<List<Double>>();
    try
    {
        using StreamReader reader = new StreamReader(path);

        while (reader.EndOfStream == false)
        {
            string text = reader.ReadLine();
            string[] spl = text.Split(" ");

            List<Double> row = new List<Double>();

            foreach (string s in spl)
            {
                row.Add(Double.Parse(s));
            }
            wielomiany.Add(row);
        }
        Console.WriteLine("-- załadowano wielomiany pomyślnie");
        return wielomiany;
    }
    catch (IOException e)
    {
        Console.WriteLine("The file could not be read:");
        Console.WriteLine(e.Message);
        return null;
    }
}

static void writePolynomial(List<List<Double>> wielomiany)
{
    Console.WriteLine("Wielomiany:");
    foreach (List<Double> row in wielomiany)
    {
        foreach (Double d in row)
        {
            Console.Write(" " + d);
        }
        Console.WriteLine();
    }
}

static void writePopulation(List<List<Double>> populacja)
{
    foreach (List<Double> item in populacja)
    {
        foreach (Double val in item)
        {
            Console.Write(val.ToString("0.00") + " ");
        }
        Console.WriteLine();
    }
}

static void writeDopasowanie(List<Double> dopasowanie)
{
    foreach (Double item in dopasowanie)
    {
        Console.Write(item.ToString("0.00") + " ");
    }
    Console.WriteLine();
}

static List<List<Double>> createZeroPopulation(int S, int N)
{
    Random rnd = new Random();
    List<List<Double>> populacjaZero = new List<List<Double>>();
    int i = 0, j = 0;
    while (i < N)
    {
        List<Double> row = new List<Double>();
        j = 0;
        while (j < S)
        {
            row.Add(rnd.NextDouble() * 2 - 1);
            j++;
        }
        populacjaZero.Add(row);
        i++;
    }
    return populacjaZero;
}

static List<Double> dopasowanie(List<List<Double>> populacja, List<List<Double>> wielomiany)
{
    List<Double> dopasowanie = new List<Double>();

    int len = wielomiany.Count();
    int i, j;

    foreach (List<Double> osobnik in populacja)
    {
        Double wartosc = new Double();
        
        wartosc = 0;
        i = 0;
        while(i < len)// pojedynczy wielomian
        {
            Double suma = new Double();
            suma = 0;
            j = 0;    
            while (j+1 < len) // wyliczanie wielomianu
            {
                suma = (suma + wielomiany[i][j]) * osobnik[i];
                j++;
            }
            suma += wielomiany[i][j];
            wartosc += suma;
            i++;
        }
        dopasowanie.Add(wartosc);
    }
    return dopasowanie;
}

static List<List<Double>> generatePopulation(List<List<Double>> staraPopulacja, int N, int S, Double T)
{
    Random rnd = new Random();
    List<List<Double>> populacjaNowa = new List<List<Double>>();

    int m, n, r;
    int i = 0, j;

    while (i < N)// krzyżowanie
    {
        m = rnd.Next(0,N);
        do
        {
            n = rnd.Next(0, N);
        } while (n == m);

        r = rnd.Next(0,S-1);

        // Krzyżowanie wedlug reguły
        List<Double> osobnik = new List<double>();
        j = 0;
        while ( j < r )
        {
            osobnik.Add(staraPopulacja[m][j]);
            j++;
        }
        while (j < S)
        {
            osobnik.Add(staraPopulacja[n][j]);
            j++;
        }
        populacjaNowa.Add(osobnik);

        i++;
    }

    // mutacje
    Double p;

    i = 0;
    while (i < N)
    {
        j = 0;
        while (j < S)
        {
            p = rnd.NextDouble();
            if (p < T)
            {
                populacjaNowa[i][j] = rnd.NextDouble() * 2 - 1;
            }
            j++;
        }
        i++;
    }

    return populacjaNowa;
}

static List<List<Double>> optimizePopulation(List<List<Double>> olderPopulation, List<List<Double>> lastPopulation, List<Double> dopasowanieOld, List<Double> dopasowanieLast, int N)
{
    var combinedFitness = new List<(double Fitness, int Index, bool IsOld)>();

    for (int i = 0; i < N; i++)
    {
        combinedFitness.Add((dopasowanieOld[i], i, true));
    }

    for (int i = 0; i < N; i++)
    {
        combinedFitness.Add((dopasowanieLast[i], i, false));
    }

    combinedFitness.Sort((a, b) => a.Fitness.CompareTo(b.Fitness));

    List<List<Double>> optimized = new List<List<Double>>();
    for (int i = 0; i < N; i++)
    {
        var (fitness, index, isOld) = combinedFitness[i];
        if (isOld)
        {
            optimized.Add(olderPopulation[index]);
        }
        else
        {
            optimized.Add(lastPopulation[index]);
        }
    }

    return optimized;
}

// START
Double T = 0.2; // wspł. mutacji 0 - brak mutacji
int max_k = 1024; // number of iterations
int S = 1024; //number of polynomials
int k = 0; // algorithm iteration
int i = 0; // iterator
int N = 5; // number of individuals
const string pathToFile = "computeFile.txt";// "C:\\Users\\smate\\Documents\\TestFile.txt";
List<List<List<Double>>> populacje = new List<List<List<Double>>>(); // k N S
List<List<Double>> wielomiany = openFile(pathToFile);
    //writePolynomial(wielomiany);
List<List<Double>> populacjaZero = createZeroPopulation(S, N);
populacje.Add(populacjaZero);
Console.WriteLine("Populacja 0:");
    //writePopulation(populacje[0]);
Console.WriteLine();
List<List<Double>> dopasowania = new List<List<double>>();
dopasowania.Add(dopasowanie(populacje[k], wielomiany));
writeDopasowanie(dopasowania[k]);
k = 1;

// end of initialization
Stopwatch stopwatch = Stopwatch.StartNew();

while (k < max_k)
{
    populacje.Add(generatePopulation(populacje[k-1], N, S, T)); //krok 3

    dopasowania.Add(dopasowanie(populacje[k],wielomiany)); // krok 4

    populacje[k] =  optimizePopulation(populacje[k-1], populacje[k], dopasowania[k-1], dopasowania[k], N); // krok 5
    //writeDopasowanie(dopasowania[k]);
    k++; // krok 6
}
stopwatch.Stop();
Console.WriteLine($"Elapsed Time: {stopwatch.Elapsed.TotalMilliseconds} ms");
Console.WriteLine("Dopasowanie ostateczne:");
writeDopasowanie(dopasowania[k-1]);
return;