// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace MyFlatBuffer
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct RoomListAction : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static RoomListAction GetRootAsRoomListAction(ByteBuffer _bb) { return GetRootAsRoomListAction(_bb, new RoomListAction()); }
  public static RoomListAction GetRootAsRoomListAction(ByteBuffer _bb, RoomListAction obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public RoomListAction __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }


  public static void StartRoomListAction(FlatBufferBuilder builder) { builder.StartTable(0); }
  public static Offset<MyFlatBuffer.RoomListAction> EndRoomListAction(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<MyFlatBuffer.RoomListAction>(o);
  }
};


}
