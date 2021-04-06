// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace MyFlatBuffer
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct RoomState : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static RoomState GetRootAsRoomState(ByteBuffer _bb) { return GetRootAsRoomState(_bb, new RoomState()); }
  public static RoomState GetRootAsRoomState(ByteBuffer _bb, RoomState obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public RoomState __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Name { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetNameBytes() { return __p.__vector_as_span<byte>(4, 1); }
#else
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetNameArray() { return __p.__vector_as_array<byte>(4); }
  public uint Duration { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public string Password { get { int o = __p.__offset(8); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetPasswordBytes() { return __p.__vector_as_span<byte>(8, 1); }
#else
  public ArraySegment<byte>? GetPasswordBytes() { return __p.__vector_as_arraysegment(8); }
#endif
  public byte[] GetPasswordArray() { return __p.__vector_as_array<byte>(8); }
  public uint PeopleCount { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public uint MapId { get { int o = __p.__offset(12); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public MyFlatBuffer.WeatherSetting? WeatherSetting { get { int o = __p.__offset(14); return o != 0 ? (MyFlatBuffer.WeatherSetting?)(new MyFlatBuffer.WeatherSetting()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public MyFlatBuffer.RandomEvent? RandomEvent { get { int o = __p.__offset(16); return o != 0 ? (MyFlatBuffer.RandomEvent?)(new MyFlatBuffer.RandomEvent()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public MyFlatBuffer.ParticipantSetting? ParticipantSetting { get { int o = __p.__offset(18); return o != 0 ? (MyFlatBuffer.ParticipantSetting?)(new MyFlatBuffer.ParticipantSetting()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }
  public MyFlatBuffer.PresetEventSetting? PresetEventSetting { get { int o = __p.__offset(20); return o != 0 ? (MyFlatBuffer.PresetEventSetting?)(new MyFlatBuffer.PresetEventSetting()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }
  public MyFlatBuffer.PresetTaskSetting? PresetTaskSetting { get { int o = __p.__offset(22); return o != 0 ? (MyFlatBuffer.PresetTaskSetting?)(new MyFlatBuffer.PresetTaskSetting()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }
  public bool ObserverMode { get { int o = __p.__offset(24); return o != 0 ? 0!=__p.bb.Get(o + __p.bb_pos) : (bool)false; } }
  public MyFlatBuffer.RoleState? Roles(int j) { int o = __p.__offset(26); return o != 0 ? (MyFlatBuffer.RoleState?)(new MyFlatBuffer.RoleState()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int RolesLength { get { int o = __p.__offset(26); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static void StartRoomState(FlatBufferBuilder builder) { builder.StartTable(12); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(0, nameOffset.Value, 0); }
  public static void AddDuration(FlatBufferBuilder builder, uint duration) { builder.AddUint(1, duration, 0); }
  public static void AddPassword(FlatBufferBuilder builder, StringOffset passwordOffset) { builder.AddOffset(2, passwordOffset.Value, 0); }
  public static void AddPeopleCount(FlatBufferBuilder builder, uint peopleCount) { builder.AddUint(3, peopleCount, 0); }
  public static void AddMapId(FlatBufferBuilder builder, uint mapId) { builder.AddUint(4, mapId, 0); }
  public static void AddWeatherSetting(FlatBufferBuilder builder, Offset<MyFlatBuffer.WeatherSetting> weatherSettingOffset) { builder.AddStruct(5, weatherSettingOffset.Value, 0); }
  public static void AddRandomEvent(FlatBufferBuilder builder, Offset<MyFlatBuffer.RandomEvent> randomEventOffset) { builder.AddStruct(6, randomEventOffset.Value, 0); }
  public static void AddParticipantSetting(FlatBufferBuilder builder, Offset<MyFlatBuffer.ParticipantSetting> participantSettingOffset) { builder.AddOffset(7, participantSettingOffset.Value, 0); }
  public static void AddPresetEventSetting(FlatBufferBuilder builder, Offset<MyFlatBuffer.PresetEventSetting> presetEventSettingOffset) { builder.AddOffset(8, presetEventSettingOffset.Value, 0); }
  public static void AddPresetTaskSetting(FlatBufferBuilder builder, Offset<MyFlatBuffer.PresetTaskSetting> presetTaskSettingOffset) { builder.AddOffset(9, presetTaskSettingOffset.Value, 0); }
  public static void AddObserverMode(FlatBufferBuilder builder, bool observerMode) { builder.AddBool(10, observerMode, false); }
  public static void AddRoles(FlatBufferBuilder builder, VectorOffset rolesOffset) { builder.AddOffset(11, rolesOffset.Value, 0); }
  public static VectorOffset CreateRolesVector(FlatBufferBuilder builder, Offset<MyFlatBuffer.RoleState>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateRolesVectorBlock(FlatBufferBuilder builder, Offset<MyFlatBuffer.RoleState>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartRolesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<MyFlatBuffer.RoomState> EndRoomState(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<MyFlatBuffer.RoomState>(o);
  }
};


}