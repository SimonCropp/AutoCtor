﻿//HintName: OuterClass1.OuterRecord1.OuterClass2.TestRecord.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/distantcam/AutoCtor
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

partial class OuterClass1
{
	partial record OuterRecord1
	{
		partial class OuterClass2
		{
			partial record TestRecord
			{
				public TestRecord(int item)
				{
					this._item = item;
				}
			}
		}
	}
}