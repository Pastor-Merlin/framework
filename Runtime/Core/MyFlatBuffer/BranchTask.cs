// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace MyFlatBuffer
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct BranchTask : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static BranchTask GetRootAsBranchTask(ByteBuffer _bb) { return GetRootAsBranchTask(_bb, new BranchTask()); }
  public static BranchTask GetRootAsBranchTask(ByteBuffer _bb, BranchTask obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public BranchTask __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public byte GroupId { get { int o = __p.__offset(4); return o != 0 ? __p.bb.Get(o + __p.bb_pos) : (byte)0; } }
  public ulong ValidDuration { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetUlong(o + __p.bb_pos) : (ulong)0; } }
  public MyFlatBuffer.MainTask TaskType { get { int o = __p.__offset(8); return o != 0 ? (MyFlatBuffer.MainTask)__p.bb.GetSbyte(o + __p.bb_pos) : MyFlatBuffer.MainTask.KILL; } }
  public uint KillTarget(int j) { int o = __p.__offset(10); return o != 0 ? __p.bb.GetUint(__p.__vector(o) + j * 4) : (uint)0; }
  public int KillTargetLength { get { int o = __p.__offset(10); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<uint> GetKillTargetBytes() { return __p.__vector_as_span<uint>(10, 4); }
#else
  public ArraySegment<byte>? GetKillTargetBytes() { return __p.__vector_as_arraysegment(10); }
#endif
  public uint[] GetKillTargetArray() { return __p.__vector_as_array<uint>(10); }
  public uint DouseTarget(int j) { int o = __p.__offset(12); return o != 0 ? __p.bb.GetUint(__p.__vector(o) + j * 4) : (uint)0; }
  public int DouseTargetLength { get { int o = __p.__offset(12); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<uint> GetDouseTargetBytes() { return __p.__vector_as_span<uint>(12, 4); }
#else
  public ArraySegment<byte>? GetDouseTargetBytes() { return __p.__vector_as_arraysegment(12); }
#endif
  public uint[] GetDouseTargetArray() { return __p.__vector_as_array<uint>(12); }
  public MyFlatBuffer.Position? ArrivePosition(int j) { int o = __p.__offset(14); return o != 0 ? (MyFlatBuffer.Position?)(new MyFlatBuffer.Position()).__assign(__p.__vector(o) + j * 8, __p.bb) : null; }
  public int ArrivePositionLength { get { int o = __p.__offset(14); return o != 0 ? __p.__vector_len(o) : 0; } }
  public uint RescueTarget(int j) { int o = __p.__offset(16); return o != 0 ? __p.bb.GetUint(__p.__vector(o) + j * 4) : (uint)0; }
  public int RescueTargetLength { get { int o = __p.__offset(16); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<uint> GetRescueTargetBytes() { return __p.__vector_as_span<uint>(16, 4); }
#else
  public ArraySegment<byte>? GetRescueTargetBytes() { return __p.__vector_as_arraysegment(16); }
#endif
  public uint[] GetRescueTargetArray() { return __p.__vector_as_array<uint>(16); }
  public uint ArrestTarget(int j) { int o = __p.__offset(18); return o != 0 ? __p.bb.GetUint(__p.__vector(o) + j * 4) : (uint)0; }
  public int ArrestTargetLength { get { int o = __p.__offset(18); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<uint> GetArrestTargetBytes() { return __p.__vector_as_span<uint>(18, 4); }
#else
  public ArraySegment<byte>? GetArrestTargetBytes() { return __p.__vector_as_arraysegment(18); }
#endif
  public uint[] GetArrestTargetArray() { return __p.__vector_as_array<uint>(18); }

  public static Offset<MyFlatBuffer.BranchTask> CreateBranchTask(FlatBufferBuilder builder,
      byte group_id = 0,
      ulong valid_duration = 0,
      MyFlatBuffer.MainTask task_type = MyFlatBuffer.MainTask.KILL,
      VectorOffset kill_targetOffset = default(VectorOffset),
      VectorOffset douse_targetOffset = default(VectorOffset),
      VectorOffset arrive_positionOffset = default(VectorOffset),
      VectorOffset rescue_targetOffset = default(VectorOffset),
      VectorOffset arrest_targetOffset = default(VectorOffset)) {
    builder.StartTable(8);
    BranchTask.AddValidDuration(builder, valid_duration);
    BranchTask.AddArrestTarget(builder, arrest_targetOffset);
    BranchTask.AddRescueTarget(builder, rescue_targetOffset);
    BranchTask.AddArrivePosition(builder, arrive_positionOffset);
    BranchTask.AddDouseTarget(builder, douse_targetOffset);
    BranchTask.AddKillTarget(builder, kill_targetOffset);
    BranchTask.AddTaskType(builder, task_type);
    BranchTask.AddGroupId(builder, group_id);
    return BranchTask.EndBranchTask(builder);
  }

  public static void StartBranchTask(FlatBufferBuilder builder) { builder.StartTable(8); }
  public static void AddGroupId(FlatBufferBuilder builder, byte groupId) { builder.AddByte(0, groupId, 0); }
  public static void AddValidDuration(FlatBufferBuilder builder, ulong validDuration) { builder.AddUlong(1, validDuration, 0); }
  public static void AddTaskType(FlatBufferBuilder builder, MyFlatBuffer.MainTask taskType) { builder.AddSbyte(2, (sbyte)taskType, 0); }
  public static void AddKillTarget(FlatBufferBuilder builder, VectorOffset killTargetOffset) { builder.AddOffset(3, killTargetOffset.Value, 0); }
  public static VectorOffset CreateKillTargetVector(FlatBufferBuilder builder, uint[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddUint(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateKillTargetVectorBlock(FlatBufferBuilder builder, uint[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartKillTargetVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddDouseTarget(FlatBufferBuilder builder, VectorOffset douseTargetOffset) { builder.AddOffset(4, douseTargetOffset.Value, 0); }
  public static VectorOffset CreateDouseTargetVector(FlatBufferBuilder builder, uint[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddUint(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateDouseTargetVectorBlock(FlatBufferBuilder builder, uint[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartDouseTargetVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddArrivePosition(FlatBufferBuilder builder, VectorOffset arrivePositionOffset) { builder.AddOffset(5, arrivePositionOffset.Value, 0); }
  public static void StartArrivePositionVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(8, numElems, 4); }
  public static void AddRescueTarget(FlatBufferBuilder builder, VectorOffset rescueTargetOffset) { builder.AddOffset(6, rescueTargetOffset.Value, 0); }
  public static VectorOffset CreateRescueTargetVector(FlatBufferBuilder builder, uint[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddUint(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateRescueTargetVectorBlock(FlatBufferBuilder builder, uint[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartRescueTargetVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddArrestTarget(FlatBufferBuilder builder, VectorOffset arrestTargetOffset) { builder.AddOffset(7, arrestTargetOffset.Value, 0); }
  public static VectorOffset CreateArrestTargetVector(FlatBufferBuilder builder, uint[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddUint(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateArrestTargetVectorBlock(FlatBufferBuilder builder, uint[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartArrestTargetVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<MyFlatBuffer.BranchTask> EndBranchTask(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<MyFlatBuffer.BranchTask>(o);
  }
};


}
