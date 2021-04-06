// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace MyFlatBuffer
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct ExitRoomAction : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static ExitRoomAction GetRootAsExitRoomAction(ByteBuffer _bb) { return GetRootAsExitRoomAction(_bb, new ExitRoomAction()); }
  public static ExitRoomAction GetRootAsExitRoomAction(ByteBuffer _bb, ExitRoomAction obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public ExitRoomAction __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public uint RoomId { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public uint ParticipantId { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }

  public static Offset<MyFlatBuffer.ExitRoomAction> CreateExitRoomAction(FlatBufferBuilder builder,
      uint room_id = 0,
      uint participant_id = 0) {
    builder.StartTable(2);
    ExitRoomAction.AddParticipantId(builder, participant_id);
    ExitRoomAction.AddRoomId(builder, room_id);
    return ExitRoomAction.EndExitRoomAction(builder);
  }

  public static void StartExitRoomAction(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddRoomId(FlatBufferBuilder builder, uint roomId) { builder.AddUint(0, roomId, 0); }
  public static void AddParticipantId(FlatBufferBuilder builder, uint participantId) { builder.AddUint(1, participantId, 0); }
  public static Offset<MyFlatBuffer.ExitRoomAction> EndExitRoomAction(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<MyFlatBuffer.ExitRoomAction>(o);
  }
};


}