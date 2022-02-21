rrf
int h(int x);
int d(int x);
int e(int x);
int g(int x);
 
int f(int x)
{
    if (x % 2)
    {
        return 1;
    }
 
    return g(x * 3

 
int g(int x) 
{
    if (e(x)) 
    {
        int s = 0;
        for (int i = 0; i < x; i++) 
        {
            s += h(i * e(i));
        }
 
        return s;
    }
    else 
    {
        return x + 1;
    }
}
 
int e(int x) 
{
    return x ? d(x / 2) : 0;
}
 
int d(int x) 
{
    return x * x;
}
 
int h(int x) 
{
    return f(e(d(x)));
}