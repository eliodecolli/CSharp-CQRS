// Code generated by protoc-gen-go. DO NOT EDIT.
// source: MarkShipmentAsDeliveredCommand.proto

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

type MarkShipmentAsDeliveredCommand struct {
	ShipmentId           string   `protobuf:"bytes,1,opt,name=ShipmentId,proto3" json:"ShipmentId,omitempty"`
	DeliveredDate        int64    `protobuf:"varint,2,opt,name=DeliveredDate,proto3" json:"DeliveredDate,omitempty"`
	AdditionalTaxes      int32    `protobuf:"varint,3,opt,name=AdditionalTaxes,proto3" json:"AdditionalTaxes,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *MarkShipmentAsDeliveredCommand) Reset()         { *m = MarkShipmentAsDeliveredCommand{} }
func (m *MarkShipmentAsDeliveredCommand) String() string { return proto.CompactTextString(m) }
func (*MarkShipmentAsDeliveredCommand) ProtoMessage()    {}
func (*MarkShipmentAsDeliveredCommand) Descriptor() ([]byte, []int) {
	return fileDescriptor_458a3ca5840acea3, []int{0}
}

func (m *MarkShipmentAsDeliveredCommand) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_MarkShipmentAsDeliveredCommand.Unmarshal(m, b)
}
func (m *MarkShipmentAsDeliveredCommand) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_MarkShipmentAsDeliveredCommand.Marshal(b, m, deterministic)
}
func (m *MarkShipmentAsDeliveredCommand) XXX_Merge(src proto.Message) {
	xxx_messageInfo_MarkShipmentAsDeliveredCommand.Merge(m, src)
}
func (m *MarkShipmentAsDeliveredCommand) XXX_Size() int {
	return xxx_messageInfo_MarkShipmentAsDeliveredCommand.Size(m)
}
func (m *MarkShipmentAsDeliveredCommand) XXX_DiscardUnknown() {
	xxx_messageInfo_MarkShipmentAsDeliveredCommand.DiscardUnknown(m)
}

var xxx_messageInfo_MarkShipmentAsDeliveredCommand proto.InternalMessageInfo

func (m *MarkShipmentAsDeliveredCommand) GetShipmentId() string {
	if m != nil {
		return m.ShipmentId
	}
	return ""
}

func (m *MarkShipmentAsDeliveredCommand) GetDeliveredDate() int64 {
	if m != nil {
		return m.DeliveredDate
	}
	return 0
}

func (m *MarkShipmentAsDeliveredCommand) GetAdditionalTaxes() int32 {
	if m != nil {
		return m.AdditionalTaxes
	}
	return 0
}

func init() {
	proto.RegisterType((*MarkShipmentAsDeliveredCommand)(nil), "BeeGees.Commands.MarkShipmentAsDeliveredCommand")
}

func init() {
	proto.RegisterFile("MarkShipmentAsDeliveredCommand.proto", fileDescriptor_458a3ca5840acea3)
}

var fileDescriptor_458a3ca5840acea3 = []byte{
	// 151 bytes of a gzipped FileDescriptorProto
	0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0xe2, 0x52, 0xf1, 0x4d, 0x2c, 0xca,
	0x0e, 0xce, 0xc8, 0x2c, 0xc8, 0x4d, 0xcd, 0x2b, 0x71, 0x2c, 0x76, 0x49, 0xcd, 0xc9, 0x2c, 0x4b,
	0x2d, 0x4a, 0x4d, 0x71, 0xce, 0xcf, 0xcd, 0x4d, 0xcc, 0x4b, 0xd1, 0x2b, 0x28, 0xca, 0x2f, 0xc9,
	0x17, 0x12, 0x70, 0x4a, 0x4d, 0x75, 0x4f, 0x4d, 0x2d, 0xd6, 0x83, 0x0a, 0x17, 0x2b, 0x4d, 0x60,
	0xe4, 0x92, 0xc3, 0xaf, 0x55, 0x48, 0x8e, 0x8b, 0x0b, 0x26, 0xeb, 0x99, 0x22, 0xc1, 0xa8, 0xc0,
	0xa8, 0xc1, 0x19, 0x84, 0x24, 0x22, 0xa4, 0xc2, 0xc5, 0x0b, 0xd7, 0xe3, 0x92, 0x58, 0x92, 0x2a,
	0xc1, 0xa4, 0xc0, 0xa8, 0xc1, 0x1c, 0x84, 0x2a, 0x28, 0xa4, 0xc1, 0xc5, 0xef, 0x98, 0x92, 0x92,
	0x59, 0x92, 0x99, 0x9f, 0x97, 0x98, 0x13, 0x92, 0x58, 0x91, 0x5a, 0x2c, 0xc1, 0xac, 0xc0, 0xa8,
	0xc1, 0x1a, 0x84, 0x2e, 0x9c, 0xc4, 0x06, 0x76, 0xab, 0x31, 0x20, 0x00, 0x00, 0xff, 0xff, 0x61,
	0x48, 0x46, 0x81, 0xd3, 0x00, 0x00, 0x00,
}
