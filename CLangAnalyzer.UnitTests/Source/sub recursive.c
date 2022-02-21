int f(int x)
{
    if (x % 2)
    {
        return 1;
    }
 
    return g(x * 3);
}
 
int g(int x) 
{
    if (e(x)) 
    {
        return 5;
    }
    else 
    {
        return x + 1;
    }
}
 
int e(int x) 
{
    return x ? g(x / 2) : 0;
}