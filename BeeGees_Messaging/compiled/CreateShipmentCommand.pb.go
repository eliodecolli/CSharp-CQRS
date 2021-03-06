// Code generated by protoc-gen-go. DO NOT EDIT.
// source: CreateShipmentCommand.proto

package BeeGees_Commands

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

type CreateShipmentCommand struct {
	ShipName             string   `protobuf:"bytes,1,opt,name=ShipName,proto3" json:"ShipName,omitempty"`
	ShipAddress          string   `protobuf:"bytes,2,opt,name=ShipAddress,proto3" json:"ShipAddress,omitempty"`
	CustomerID           string   `protobuf:"bytes,3,opt,name=CustomerID,proto3" json:"CustomerID,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *CreateShipmentCommand) Reset()         { *m = CreateShipmentCommand{} }
func (m *CreateShipmentCommand) String() string { return proto.CompactTextString(m) }
func (*CreateShipmentCommand) ProtoMessage()    {}
func (*CreateShipmentCommand) Descriptor() ([]byte, []int) {
	return fileDescriptor_aa1b0d6659429737, []int{0}
}

func (m *CreateShipmentCommand) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_CreateShipmentCommand.Unmarshal(m, b)
}
func (m *CreateShipmentCommand) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_CreateShipmentCommand.Marshal(b, m, deterministic)
}
func (m *CreateShipmentCommand) XXX_Merge(src proto.Message) {
	xxx_messageInfo_CreateShipmentCommand.Merge(m, src)
}
func (m *CreateShipmentCommand) XXX_Size() int {
	return xxx_messageInfo_CreateShipmentCommand.Size(m)
}
func (m *CreateShipmentCommand) XXX_DiscardUnknown() {
	xxx_messageInfo_CreateShipmentCommand.DiscardUnknown(m)
}

var xxx_messageInfo_CreateShipmentCommand proto.InternalMessageInfo

func (m *CreateShipmentCommand) GetShipName() string {
	if m != nil {
		return m.ShipName
	}
	return ""
}

func (m *CreateShipmentCommand) GetShipAddress() string {
	if m != nil {
		return m.ShipAddress
	}
	return ""
}

func (m *CreateShipmentCommand) GetCustomerID() string {
	if m != nil {
		return m.CustomerID
	}
	return ""
}

func init() {
	proto.RegisterType((*CreateShipmentCommand)(nil), "BeeGees.Commands.CreateShipmentCommand")
}

func init() { proto.RegisterFile("CreateShipmentCommand.proto", fileDescriptor_aa1b0d6659429737) }

var fileDescriptor_aa1b0d6659429737 = []byte{
	// 135 bytes of a gzipped FileDescriptorProto
	0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0xe2, 0x92, 0x76, 0x2e, 0x4a, 0x4d,
	0x2c, 0x49, 0x0d, 0xce, 0xc8, 0x2c, 0xc8, 0x4d, 0xcd, 0x2b, 0x71, 0xce, 0xcf, 0xcd, 0x4d, 0xcc,
	0x4b, 0xd1, 0x2b, 0x28, 0xca, 0x2f, 0xc9, 0x17, 0x12, 0x70, 0x4a, 0x4d, 0x75, 0x4f, 0x4d, 0x2d,
	0xd6, 0x83, 0x0a, 0x17, 0x2b, 0x95, 0x72, 0x89, 0x62, 0xd5, 0x20, 0x24, 0xc5, 0xc5, 0x01, 0x12,
	0xf2, 0x4b, 0xcc, 0x4d, 0x95, 0x60, 0x54, 0x60, 0xd4, 0xe0, 0x0c, 0x82, 0xf3, 0x85, 0x14, 0xb8,
	0xb8, 0x41, 0x6c, 0xc7, 0x94, 0x94, 0xa2, 0xd4, 0xe2, 0x62, 0x09, 0x26, 0xb0, 0x34, 0xb2, 0x90,
	0x90, 0x1c, 0x17, 0x97, 0x73, 0x69, 0x71, 0x49, 0x7e, 0x6e, 0x6a, 0x91, 0xa7, 0x8b, 0x04, 0x33,
	0x58, 0x01, 0x92, 0x48, 0x12, 0x1b, 0xd8, 0x3d, 0xc6, 0x80, 0x00, 0x00, 0x00, 0xff, 0xff, 0xf6,
	0x99, 0xa5, 0x32, 0xae, 0x00, 0x00, 0x00,
}
