// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var testowa = new Testowa();
var tab=new Task<int>[10];

for(int i=0; i<10;i++)
{
    tab[i] = testowa.Kwadrat(i);
}
var result=await Task.WhenAll(tab);
foreach (var item in result)
{
    Console.WriteLine(item);
}

class Testowa
{
    public async Task<int> Kwadrat(int x) => x * x;
}