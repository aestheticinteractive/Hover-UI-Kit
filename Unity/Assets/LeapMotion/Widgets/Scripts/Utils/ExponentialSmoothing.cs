public class ExponentialSmoothing
{
  private float alpha;
  private float value = float.MinValue;

  public ExponentialSmoothing(float alpha)
  {
    this.alpha = alpha;
  }

  public float Calculate(float value)
  {
    this.value = (this.value == float.MinValue) ? value : alpha * value + (1 - alpha) * this.value;
    return this.value;
  }

  public float Value()
  {
    return this.value;
  }
}

public class ExponentialSmoothingXYZ
{
  private float alpha;
  private float X = float.MinValue;
  private float Y = float.MinValue;
  private float Z = float.MinValue;

  public ExponentialSmoothingXYZ(float alpha)
  {
    this.alpha = alpha;
  }

  public void Calculate(float X, float Y, float Z)
  {
    this.X = (this.X == float.MinValue) ? X : alpha * X + (1 - alpha) * this.X;
    this.Y = (this.Y == float.MinValue) ? Y : alpha * Y + (1 - alpha) * this.Y;
    this.Z = (this.Z == float.MinValue) ? Z : alpha * Z + (1 - alpha) * this.Z;
  }

  public float GetX()
  {
    return X;
  }

  public float GetY()
  {
    return Y;
  }

  public float GetZ()
  {
    return Z;
  }
}