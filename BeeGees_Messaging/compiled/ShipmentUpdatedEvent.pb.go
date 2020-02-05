// Code generated by protoc-gen-go. DO NOT EDIT.
// source: ShipmentUpdatedEvent.proto

package BeeGees_Events

import (
	fmt "fmt"
	proto "github.com/golang/protobuf/proto"
	math "math"
)

// Reference imports to suppress errors if they are not otherwise used.
var _ = proto.Marshal
var _ = fmt.Errorf
var _ = math.Inf

// This is a compile-time assertion to ensure that this generated file
// is compatible with the proto package it is being compiled against.
// A compilation error at this line likely means your copy of the
// proto package needs to be updated.
const _ = proto.ProtoPackageIsVersion3 // please upgrade the proto package

type ShipmentUpdatedEvent struct {
	ShipmentId           string   `protobuf:"bytes,1,opt,name=ShipmentId,proto3" json:"ShipmentId,omitempty"`
	Status               string   `protobuf:"bytes,2,opt,name=Status,proto3" json:"Status,omitempty"`
	Location             string   `protobuf:"bytes,3,opt,name=Location,proto3" json:"Location,omitempty"`
	LastUpdated          int64    `protobuf:"varint,4,opt,name=LastUpdated,proto3" json:"LastUpdated,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *ShipmentUpdatedEvent) Reset()         { *m = ShipmentUpdatedEvent{} }
func (m *ShipmentUpdatedEvent) String() string { return proto.CompactTextString(m) }
func (*ShipmentUpdatedEvent) ProtoMessage()    {}
func (*ShipmentUpdatedEvent) Descriptor() ([]byte, []int) {
	return fileDescriptor_6f16f8e51f56e787, []int{0}
}

func (m *ShipmentUpdatedEvent) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_ShipmentUpdatedEvent.Unmarshal(m, b)
}
func (m *ShipmentUpdatedEvent) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_ShipmentUpdatedEvent.Marshal(b, m, deterministic)
}
func (m *ShipmentUpdatedEvent) XXX_Merge(src proto.Message) {
	xxx_messageInfo_ShipmentUpdatedEvent.Merge(m, src)
}
func (m *ShipmentUpdatedEvent) XXX_Size() int {
	return xxx_messageInfo_ShipmentUpdatedEvent.Size(m)
}
func (m *ShipmentUpdatedEvent) XXX_DiscardUnknown() {
	xxx_messageInfo_ShipmentUpdatedEvent.DiscardUnknown(m)
}

var xxx_messageInfo_ShipmentUpdatedEvent proto.InternalMessageInfo

func (m *ShipmentUpdatedEvent) GetShipmentId() string {
	if m != nil {
		return m.ShipmentId
	}
	return ""
}

func (m *ShipmentUpdatedEvent) GetStatus() string {
	if m != nil {
		return m.Status
	}
	return ""
}

func (m *ShipmentUpdatedEvent) GetLocation() string {
	if m != nil {
		return m.Location
	}
	return ""
}

func (m *ShipmentUpdatedEvent) GetLastUpdated() int64 {
	if m != nil {
		return m.LastUpdated
	}
	return 0
}

func init() {
	proto.RegisterType((*ShipmentUpdatedEvent)(nil), "BeeGees.Events.ShipmentUpdatedEvent")
}

func init() { proto.RegisterFile("ShipmentUpdatedEvent.proto", fileDescriptor_6f16f8e51f56e787) }

var fileDescriptor_6f16f8e51f56e787 = []byte{
	// 146 bytes of a gzipped FileDescriptorProto
	0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0xe2, 0x92, 0x0a, 0xce, 0xc8, 0x2c,
	0xc8, 0x4d, 0xcd, 0x2b, 0x09, 0x2d, 0x48, 0x49, 0x2c, 0x49, 0x4d, 0x71, 0x2d, 0x4b, 0xcd, 0x2b,
	0xd1, 0x2b, 0x28, 0xca, 0x2f, 0xc9, 0x17, 0xe2, 0x73, 0x4a, 0x4d, 0x75, 0x4f, 0x4d, 0x2d, 0xd6,
	0x03, 0x0b, 0x16, 0x2b, 0xf5, 0x30, 0x72, 0x89, 0x60, 0x53, 0x2e, 0x24, 0xc7, 0xc5, 0x05, 0x13,
	0xf7, 0x4c, 0x91, 0x60, 0x54, 0x60, 0xd4, 0xe0, 0x0c, 0x42, 0x12, 0x11, 0x12, 0xe3, 0x62, 0x0b,
	0x2e, 0x49, 0x2c, 0x29, 0x2d, 0x96, 0x60, 0x02, 0xcb, 0x41, 0x79, 0x42, 0x52, 0x5c, 0x1c, 0x3e,
	0xf9, 0xc9, 0x89, 0x25, 0x99, 0xf9, 0x79, 0x12, 0xcc, 0x60, 0x19, 0x38, 0x5f, 0x48, 0x81, 0x8b,
	0xdb, 0x27, 0xb1, 0x18, 0x66, 0x8f, 0x04, 0x8b, 0x02, 0xa3, 0x06, 0x73, 0x10, 0xb2, 0x50, 0x12,
	0x1b, 0xd8, 0x95, 0xc6, 0x80, 0x00, 0x00, 0x00, 0xff, 0xff, 0x38, 0xe7, 0x0a, 0xd7, 0xc3, 0x00,
	0x00, 0x00,
}
