using System.Buffers;
using System.Globalization;
using System.Text;

namespace AFFKit.Core.Utility.Text;

/// <summary>
/// Represents a parser for reading values from a span-based string.
/// </summary>
/// <seealso cref="StringParser"/>
public ref struct StackStringParser
{
	private readonly static SearchValues<char> _terminators = StringParser.Terminators;
	private ReadOnlySpan<char> _source;

	public Rune Current => FetchRune(_source);

	public StackStringParser(ReadOnlySpan<char> source)
	{
		_source = source;
	}

	public StackStringParser(string source)
	{
		_source = source.AsSpan();
	}

	[Obsolete("Use SkipRunes instead. This method uses UTF-16 character count which may lead to incorrect behavior with characters using surrogate pairs like emojis and rare CJK characters.")]
	public void Skip(int length)
	{
		if (length > _source.Length) throw new IndexOutOfRangeException("Cannot skip beyond the end of the source span string.");
		_source = _source[length..];
	}

	public void SkipRunes(int count)
	{
		for (int i = 0; i < count; i++)
		{
			if (_source.IsEmpty) break;
			Rune.DecodeFromUtf16(_source, out _, out int charsConsumed);
			_source = _source[charsConsumed..];
		}
	}

	public float ReadFloat(CultureInfo? cultureInfo = null)
	{
		return float.Parse(ReadSpan(), cultureInfo ?? CultureInfo.InvariantCulture);
	}

	public int ReadInt(CultureInfo? cultureInfo = null)
	{
		return int.Parse(ReadSpan(), cultureInfo ?? CultureInfo.InvariantCulture);
	}

	public bool ReadBool()
	{
		var span = ReadSpan();

		if (span.Equals("true", StringComparison.OrdinalIgnoreCase)) return true;
		if (span.Equals("false", StringComparison.OrdinalIgnoreCase)) return false;

		throw new FormatException("Invalid boolean string format.");
	}

	public bool TryReadFloat(out float value, CultureInfo? cultureInfo = null)
	{
		var test = this;
		try
		{
			var span = test.ReadSpan();
			if (float.TryParse(span, NumberStyles.Float | NumberStyles.AllowThousands, cultureInfo ?? CultureInfo.InvariantCulture, out value))
			{
				ReadSpan(); // Here we ensure ReadSpan will not throw any exception because if it will throw, it should already being thrown on the test instance, and be caught by the catch block.
				// (same as below)
				return true;
			}
		}
		catch (FormatException)
		{
			// ignore
		}

		value = 0;
		return false;
	}

	public bool TryReadInt(out int value, CultureInfo? cultureInfo = null)
	{
		var test = this;
		try
		{
			var span = test.ReadSpan();
			if (int.TryParse(span, NumberStyles.Integer, cultureInfo ?? CultureInfo.InvariantCulture, out value))
			{
				ReadSpan();
				return true;
			}
		}
		catch (FormatException)
		{
			// ignore
		}

		value = 0;
		return false;
	}

	public bool TryReadBool(out bool value)
	{
		var test = this;
		try
		{
			var span = test.ReadSpan();

			if (span.Equals("true", StringComparison.OrdinalIgnoreCase))
			{
				value = true;
				ReadSpan();
				return true;
			}

			if (span.Equals("false", StringComparison.OrdinalIgnoreCase))
			{
				value = false;
				ReadSpan();
				return true;
			}
		}
		catch (FormatException)
		{
			// ignore
		}

		value = false;
		return false;
	}

	public string ReadString()
	{
		return ReadSpan().ToString();
	}

	public string ReadString(out bool isComma)
	{
		var span = ReadSpan(out char terminator);
		isComma = terminator == ',';
		return span.ToString();
	}

	public char Peek(int count = 1)
	{
		if (count < 1)
		{
			throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
		}

		if (_source.Length <= count)
		{
			throw new IndexOutOfRangeException("Cannot peek beyond the end of the source span string.");
		}

		return _source[count - 1];
	}

	public Rune PeekRune(int count = 1)
	{
		if (count < 1)
		{
			throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
		}

		var temp = _source;
		var result = new Rune('\0');

		for (int i = 0; i < count; i++)
		{
			if (temp.IsEmpty) throw new IndexOutOfRangeException("Cannot peek beyond the end of the source span string.");
			Rune.DecodeFromUtf16(temp, out result, out int charsConsumed);
			temp = temp[charsConsumed..];
		}

		return result;
	}

	private ReadOnlySpan<char> ReadSpan()
	{
		return ReadSpan(out _);
	}

	private ReadOnlySpan<char> ReadSpan(out char terminator)
	{
		int idx = _source.IndexOfAny(_terminators);

		if (idx == -1)
		{
			throw new FormatException("Terminator not found in the source span string.");
		}

		var result = _source[..idx];
		terminator = _source[idx];
		_source = _source[(idx + 1)..];
		return result;
	}

	private static Rune FetchRune(ReadOnlySpan<char> span)
	{
		if (span.IsEmpty) return new Rune('\0');

		if (Rune.DecodeFromUtf16(span, out var rune, out _) == OperationStatus.Done)
		{
			return rune;
		}

		throw new FormatException("Failed to decode Rune from the provided span.");
	}
}