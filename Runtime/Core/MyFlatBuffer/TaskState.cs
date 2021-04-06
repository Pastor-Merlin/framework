// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace MyFlatBuffer
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct TaskState : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static TaskState GetRootAsTaskState(ByteBuffer _bb) { return GetRootAsTaskState(_bb, new TaskState()); }
  public static TaskState GetRootAsTaskState(ByteBuffer _bb, TaskState obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public TaskState __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public MyFlatBuffer.TaskStatus State { get { int o = __p.__offset(4); return o != 0 ? (MyFlatBuffer.TaskStatus)__p.bb.GetSbyte(o + __p.bb_pos) : MyFlatBuffer.TaskStatus.UNDONE; } }

  public static Offset<MyFlatBuffer.TaskState> CreateTaskState(FlatBufferBuilder builder,
      MyFlatBuffer.TaskStatus state = MyFlatBuffer.TaskStatus.UNDONE) {
    builder.StartTable(1);
    TaskState.AddState(builder, state);
    return TaskState.EndTaskState(builder);
  }

  public static void StartTaskState(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddState(FlatBufferBuilder builder, MyFlatBuffer.TaskStatus state) { builder.AddSbyte(0, (sbyte)state, 0); }
  public static Offset<MyFlatBuffer.TaskState> EndTaskState(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<MyFlatBuffer.TaskState>(o);
  }
};


}
