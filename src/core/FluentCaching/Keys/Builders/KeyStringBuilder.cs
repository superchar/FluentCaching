using System;
using System.Buffers;
using FluentCaching.Keys.Exceptions;

namespace FluentCaching.Keys.Builders;

internal struct KeyStringBuilder : IDisposable
{
   private const int MaxLength = 120;
   
   private readonly string[] _currentArray;
   private int _currentLength;

   public KeyStringBuilder()
   {
      _currentArray = ArrayPool<string>.Shared.Rent(MaxLength);
      _currentLength = 0;
   }

   public void Append(string value)
   {
      _currentLength++;
      if (_currentLength > MaxLength)
      {
         throw new KeyCountExceededException(MaxLength);
      }

      _currentArray[_currentLength - 1] = value;
   }
   
   public override string ToString()
      => string.Join(string.Empty, _currentArray, 0, _currentLength);

   public void Dispose()
      => ArrayPool<string>.Shared.Return(_currentArray);
}
