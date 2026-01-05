using System.ComponentModel;
using AFFKit.Core.Data.Chart;

namespace AFFKit.Core.Enumerators;

public enum ArcType
{
	/// <summary>
	/// Solid arc (with judgments).<br />
	/// <b>No</b> <see cref="ArcTap"/>s can be placed on this arc.
	/// </summary>
	[Description("false")] Solid = 0,
	
	/// <summary>
	/// Regular trace arc (all types of trace arcs are without judgments).<br />
	/// <see cref="ArcTap"/>s can be placed on this arc.
	/// </summary>
	[Description("true")] TraceVoid = 1,
	
	/// <summary>
	/// Special red trace arc used in Special Scenes of <c>Lament Rain</c> and <c>Designant.</c>.<br />
	/// <see cref="ArcTap"/>s on this arc will also be rendered in red and <c>NOT</c> being calculated into the combos and the score.
	/// </summary>
	[Description("designant")] TraceDesignant = 2
}

public enum ArcLineType
{
	[Description("s")] Straight = 0,
	[Description("b")] Bezier = 1,
	[Description("si")] Si = 2,
	[Description("so")] So = 3,
	[Description("sisi")] SiSi = 4,
	[Description("siso")] SiSo = 5,
	[Description("sosi")] SoSi = 6,
	[Description("soso")] SoSo = 7,
}