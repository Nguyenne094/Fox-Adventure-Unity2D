using System;

public class ProgressInformation : IProgress<float>
{
    public event Action<float> OnProgressChanged = delegate { };

    private int _total;
    
    public void Report(float value)
    {
        OnProgressChanged?.Invoke(value/_total);
    }
}