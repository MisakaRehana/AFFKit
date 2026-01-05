using System.Buffers;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace AFFKit.Core.Utility.Text;

/// <summary>
/// Represents a parser for reading various data types from a string sequentially.
/// </summary>
/// <seealso cref="StackStringParser"/>
public sealed class StringParser : IDisposable
{
	public readonly static SearchValues<char> Terminators = SearchValues.Create([',', ')']);
	private ReadOnlyMemory<char> _source;
	private bool isDisposed;
	
	public Rune Current => FetchRune(_source.Span);
	
	public StringParser(string source)
	{
		_source = source.AsMemory();
		isDisposed = false;
	}
	
	public StringParser(ReadOnlySpan<char> source)
	{
		_source = source.ToArray();
		isDisposed = false;
	}
	
	public StringParser(ReadOnlyMemory<char> source)
	{
		_source = source;
		isDisposed = false;
	}

	public StringParser(Span<char> source)
	{
		Debug.WriteLine("StringParser warning: Initializing from Span<char> will fix the source string into a read-only memory array.\n" +
		                "Consider using ReadOnlySpan<char> to avoid unnecessary allocations.");
		_source = source.ToArray();
		isDisposed = false;
	}
	
	public StringParser(Memory<char> source)
	{
		Debug.WriteLine("StringParser warning: Initializing from Memory<char> will fix the source string into a read-only memory array.\n" +
		                "Consider using ReadOnlyMemory<char> to avoid unnecessary allocations.");
		_source = source.ToArray();
		isDisposed = false;
	}
	
	[Obsolete("Use SkipRunes instead. This method uses UTF-16 character count which may lead to incorrect behavior with characters using surrogate pairs like emojis and rare CJK characters.")]
	public void Skip(int length)
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		if (length > _source.Length) throw new IndexOutOfRangeException("Cannot skip beyond the end of the source string.");
		_source = _source[length..];
	}

	public void SkipRunes(int count)
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		var span = _source.Span;
		int totalChars = 0;

		for (int i = 0; i < count; i++)
		{
			if (span.IsEmpty) break;
			Rune.DecodeFromUtf16(span, out _, out int charsConsumed);
			span = span[charsConsumed..];
			totalChars += charsConsumed;
		}
		
		_source = _source[totalChars..];
	}
	
	public float ReadFloat(CultureInfo? cultureInfo = null)
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		return float.Parse(ReadSpan(), cultureInfo ?? CultureInfo.InvariantCulture);
	}
	
	public int ReadInt(CultureInfo? cultureInfo = null)
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		return int.Parse(ReadSpan(), cultureInfo ?? CultureInfo.InvariantCulture);
	}
	
	public bool ReadBool()
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		var span = ReadSpan();
		
		if (span.Equals("true", StringComparison.OrdinalIgnoreCase)) return true;
		if (span.Equals("false", StringComparison.OrdinalIgnoreCase)) return false;
		
		throw new FormatException("Invalid boolean string format.");
	}
	
	public bool TryReadFloat(out float value, CultureInfo? cultureInfo = null)
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		var backup = _source; // Memory is value type, so this creates a copy
		try
		{
			var span = ReadSpan();
			if (float.TryParse(span, NumberStyles.Integer, cultureInfo ?? CultureInfo.InvariantCulture, out value))
			{
				return true;
			}
		}
		catch (FormatException)
		{
			// ignore
		}
		
		_source = backup; // restore
		value = 0;
		return false;
	}
	
	public bool TryReadInt(out int value, CultureInfo? cultureInfo = null)
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		var backup = _source; // Memory is value type, so this creates a copy
		try
		{
			var span = ReadSpan();
			if (int.TryParse(span, NumberStyles.Integer, cultureInfo ?? CultureInfo.InvariantCulture, out value))
			{
				return true;
			}
		}
		catch (FormatException)
		{
			// ignore
		}
		
		_source = backup; // restore
		value = 0;
		return false;
	}
	
	public bool TryReadBool(out bool value)
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		var backup = _source; // Memory is value type, so this creates a copy
		try
		{
			var span = ReadSpan();
			
			if (span.Equals("true", StringComparison.OrdinalIgnoreCase))
			{
				value = true;
				return true;
			}
			if (span.Equals("false", StringComparison.OrdinalIgnoreCase))
			{
				value = false;
				return true;
			}
		}
		catch (FormatException)
		{
			// ignore
		}
		
		_source = backup; // restore
		value = false;
		return false;
	}
	
	public string ReadString()
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		return ReadSpan().ToString();
	}
	
	public string ReadString(out bool isComma)
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		var span = ReadSpan(out char terminator);
		isComma = terminator == ',';
		return span.ToString();
	}
	
	public StackStringParser Snapshot()
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		return new StackStringParser(_source.Span);
	}
	
	public ReadOnlySpan<char> ReadSpan()
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		return ReadSpan(out _);
	}
	
	private ReadOnlySpan<char> ReadSpan(out char terminator)
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		int idx = _source.Span.IndexOfAny(Terminators);

		if (idx == -1)
		{
			throw new FormatException("Terminator not found in the source string.");
		}

		var result = _source.Span[..idx];
		terminator = _source.Span[idx];
		_source = _source[(idx + 1)..];
		return result;
	}

	public void Dispose()
	{
		if (isDisposed) return;
		_source = ReadOnlyMemory<char>.Empty;
		isDisposed = true;
	}
	
	private static Rune FetchRune(ReadOnlySpan<char> span)
	{
		if (span.IsEmpty) throw new IndexOutOfRangeException("Cannot fetch rune from an empty span.");
		if (Rune.DecodeFromUtf16(span, out var result, out _ ) != OperationStatus.Done)
		{
			throw new FormatException("Invalid UTF-16 encoding in the source span.");
		}
		
		return result;
	}
}