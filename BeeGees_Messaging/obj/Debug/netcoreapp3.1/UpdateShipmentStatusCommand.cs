// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Commands/UpdateShipmentStatusCommand.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace BeeGees.Commands {

  /// <summary>Holder for reflection information generated from Commands/UpdateShipmentStatusCommand.proto</summary>
  public static partial class UpdateShipmentStatusCommandReflection {

    #region Descriptor
    /// <summary>File descriptor for Commands/UpdateShipmentStatusCommand.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static UpdateShipmentStatusCommandReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CipDb21tYW5kcy9VcGRhdGVTaGlwbWVudFN0YXR1c0NvbW1hbmQucHJvdG8S",
            "EEJlZUdlZXMuQ29tbWFuZHMiTQoVVXBkYXRlU2hpcG1lbnRDb21tYW5kEhIK",
            "ClNoaXBtZW50SWQYASABKAkSDgoGU3RhdHVzGAIgASgJEhAKCExvY2F0aW9u",
            "GAMgASgJYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::BeeGees.Commands.UpdateShipmentCommand), global::BeeGees.Commands.UpdateShipmentCommand.Parser, new[]{ "ShipmentId", "Status", "Location" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class UpdateShipmentCommand : pb::IMessage<UpdateShipmentCommand> {
    private static readonly pb::MessageParser<UpdateShipmentCommand> _parser = new pb::MessageParser<UpdateShipmentCommand>(() => new UpdateShipmentCommand());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UpdateShipmentCommand> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::BeeGees.Commands.UpdateShipmentStatusCommandReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpdateShipmentCommand() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpdateShipmentCommand(UpdateShipmentCommand other) : this() {
      shipmentId_ = other.shipmentId_;
      status_ = other.status_;
      location_ = other.location_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpdateShipmentCommand Clone() {
      return new UpdateShipmentCommand(this);
    }

    /// <summary>Field number for the "ShipmentId" field.</summary>
    public const int ShipmentIdFieldNumber = 1;
    private string shipmentId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ShipmentId {
      get { return shipmentId_; }
      set {
        shipmentId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Status" field.</summary>
    public const int StatusFieldNumber = 2;
    private string status_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Status {
      get { return status_; }
      set {
        status_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Location" field.</summary>
    public const int LocationFieldNumber = 3;
    private string location_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Location {
      get { return location_; }
      set {
        location_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UpdateShipmentCommand);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UpdateShipmentCommand other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ShipmentId != other.ShipmentId) return false;
      if (Status != other.Status) return false;
      if (Location != other.Location) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ShipmentId.Length != 0) hash ^= ShipmentId.GetHashCode();
      if (Status.Length != 0) hash ^= Status.GetHashCode();
      if (Location.Length != 0) hash ^= Location.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ShipmentId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(ShipmentId);
      }
      if (Status.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Status);
      }
      if (Location.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Location);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ShipmentId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ShipmentId);
      }
      if (Status.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Status);
      }
      if (Location.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Location);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UpdateShipmentCommand other) {
      if (other == null) {
        return;
      }
      if (other.ShipmentId.Length != 0) {
        ShipmentId = other.ShipmentId;
      }
      if (other.Status.Length != 0) {
        Status = other.Status;
      }
      if (other.Location.Length != 0) {
        Location = other.Location;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            ShipmentId = input.ReadString();
            break;
          }
          case 18: {
            Status = input.ReadString();
            break;
          }
          case 26: {
            Location = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code