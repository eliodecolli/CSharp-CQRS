// Code generated by protoc-gen-go. DO NOT EDIT.
// source: ShipmentCreatedEvent.proto

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

type ShipmentCreatedEvent struct {
	ShipmentId           string   `protobuf:"bytes,1,opt,name=ShipmentId,proto3" json:"ShipmentId,omitempty"`
	CustomerId           string   `protobuf:"bytes,2,opt,name=CustomerId,proto3" json:"CustomerId,omitempty"`
	ShipmentAddress      string   `protobuf:"bytes,3,opt,name=ShipmentAddress,proto3" json:"ShipmentAddress,omitempty"`
	CurrentLocation      string   `protobuf:"bytes,4,opt,name=CurrentLocation,proto3" json:"CurrentLocation,omitempty"`
	Status               string   `protobuf:"bytes,5,opt,name=Status,proto3" json:"Status,omitempty"`
	LastUpdated          int64    `protobuf:"varint,6,opt,name=LastUpdated,proto3" json:"LastUpdated,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *ShipmentCreatedEvent) Reset()         { *m = ShipmentCreatedEvent{} }
func (m *ShipmentCreatedEvent) String() string { return proto.CompactTextString(m) }
func (*ShipmentCreatedEvent) ProtoMessage()    {}
func (*ShipmentCreatedEvent) Descriptor() ([]byte, []int) {
	return fileDescriptor_973953bc56a9262c, []int{0}
}

func (m *ShipmentCreatedEvent) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_ShipmentCreatedEvent.Unmarshal(m, b)
}
func (m *ShipmentCreatedEvent) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_ShipmentCreatedEvent.Marshal(b, m, deterministic)
}
func (m *ShipmentCreatedEvent) XXX_Merge(src proto.Message) {
	xxx_messageInfo_ShipmentCreatedEvent.Merge(m, src)
}
func (m *ShipmentCreatedEvent) XXX_Size() int {
	return xxx_messageInfo_ShipmentCreatedEvent.Size(m)
}
func (m *ShipmentCreatedEvent) XXX_DiscardUnknown() {
	xxx_messageInfo_ShipmentCreatedEvent.DiscardUnknown(m)
}

var xxx_messageInfo_ShipmentCreatedEvent proto.InternalMessageInfo

func (m *ShipmentCreatedEvent) GetShipmentId() string {
	if m != nil {
		return m.ShipmentId
	}
	return ""
}

func (m *ShipmentCreatedEvent) GetCustomerId() string {
	if m != nil {
		return m.CustomerId
	}
	return ""
}

func (m *ShipmentCreatedEvent) GetShipmentAddress() string {
	if m != nil {
		return m.ShipmentAddress
	}
	return ""
}

func (m *ShipmentCreatedEvent) GetCurrentLocation() string {
	if m != nil {
		return m.CurrentLocation
	}
	return ""
}

func (m *ShipmentCreatedEvent) GetStatus() string {
	if m != nil {
		return m.Status
	}
	return ""
}

func (m *ShipmentCreatedEvent) GetLastUpdated() int64 {
	if m != nil {
		return m.LastUpdated
	}
	return 0
}

func init() {
	proto.RegisterType((*ShipmentCreatedEvent)(nil), "BeeGees.Events.ShipmentCreatedEvent")
}

func init() { proto.RegisterFile("ShipmentCreatedEvent.proto", fileDescriptor_973953bc56a9262c) }

var fileDescriptor_973953bc56a9262c = []byte{
	// 192 bytes of a gzipped FileDescriptorProto
	0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0x6c, 0xcf, 0xb1, 0xaa, 0xc2, 0x30,
	0x14, 0xc6, 0x71, 0x72, 0x7b, 0x2d, 0x18, 0x41, 0x21, 0x88, 0x04, 0x07, 0x29, 0x4e, 0x9d, 0xba,
	0xf8, 0x04, 0x5a, 0x44, 0x0a, 0x9d, 0x5a, 0x7c, 0x80, 0x68, 0x0e, 0xd8, 0xa1, 0x49, 0xc9, 0x39,
	0xf5, 0x89, 0x7d, 0x10, 0x69, 0x6a, 0xb1, 0x14, 0xc7, 0xfc, 0xbe, 0xff, 0x90, 0xc3, 0xb7, 0xe5,
	0xa3, 0x6a, 0x6a, 0x30, 0x94, 0x3a, 0x50, 0x04, 0xfa, 0xfc, 0x04, 0x43, 0x49, 0xe3, 0x2c, 0x59,
	0xb1, 0x3c, 0x01, 0x5c, 0x00, 0x30, 0xf1, 0x88, 0xfb, 0x17, 0xe3, 0xeb, 0x5f, 0xb9, 0xd8, 0x71,
	0x3e, 0x78, 0xa6, 0x25, 0x8b, 0x58, 0x3c, 0x2f, 0x46, 0xd2, 0xed, 0x69, 0x8b, 0x64, 0x6b, 0x70,
	0x99, 0x96, 0x7f, 0xfd, 0xfe, 0x15, 0x11, 0xf3, 0xd5, 0x50, 0x1f, 0xb5, 0x76, 0x80, 0x28, 0x03,
	0x1f, 0x4d, 0xb9, 0x2b, 0xd3, 0xd6, 0x39, 0x30, 0x94, 0xdb, 0xbb, 0xa2, 0xca, 0x1a, 0xf9, 0xdf,
	0x97, 0x13, 0x16, 0x1b, 0x1e, 0x96, 0xa4, 0xa8, 0x45, 0x39, 0xf3, 0xc1, 0xe7, 0x25, 0x22, 0xbe,
	0xc8, 0x15, 0xd2, 0xb5, 0xd1, 0xdd, 0xff, 0x65, 0x18, 0xb1, 0x38, 0x28, 0xc6, 0x74, 0x0b, 0xfd,
	0xf5, 0x87, 0x77, 0x00, 0x00, 0x00, 0xff, 0xff, 0x70, 0x25, 0xae, 0xdd, 0x1b, 0x01, 0x00, 0x00,
}
