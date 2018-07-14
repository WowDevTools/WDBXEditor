using System;

namespace WDBXEditor.Archives.CASC.Constants
{
	[Flags]
	public enum Locales : uint
	{
		All = 0xFFFFFFFF,
		None = 0,
		EnUS = 0x2,
		KoKR = 0x4,
		FrFR = 0x10,
		DeDE = 0x20,
		ZhCN = 0x40,
		EsES = 0x80,
		ZhTW = 0x100,
		EnGB = 0x200,
		EnCN = 0x400,
		EnTW = 0x800,
		EsMX = 0x1000,
		RuRU = 0x2000,
		PtBR = 0x4000,
		ItIT = 0x8000,
		PtPT = 0x10000,
		All_WoW = EnUS | KoKR | FrFR | DeDE | ZhCN | EsES | ZhTW | EnGB | EsMX | RuRU | PtBR | ItIT | PtPT
	}
}
