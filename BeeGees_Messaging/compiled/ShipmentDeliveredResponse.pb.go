// Code generated by protoc-gen-go. DO NOT EDIT.
// source: ShipmentDeliveredResponse.proto

package BeeGees_Commands_Responses

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

type ShipmentDeliveredResponse struct {
	ShipmentId           string   `protobuf:"bytes,1,opt,name=ShipmentId,proto3" json:"ShipmentId,omitempty"`
	ExtraFees            int32    `protobuf:"varint,2,opt,name=ExtraFees,proto3" json:"ExtraFees,omitempty"`
	Success              bool     `protobuf:"varint,3,opt,name=Success,proto3" json:"Success,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *ShipmentDeliveredResponse) Reset()         { *m = ShipmentDeliveredResponse{} }
func (m *ShipmentDeliveredResponse) String() string { return proto.CompactTextString(m) }
func (*ShipmentDeliveredResponse) ProtoMessage()    {}
func (*ShipmentDeliveredResponse) Descriptor() ([]byte, []int) {
	return fileDescriptor_7d3a34b25f17d508, []int{0}
}

func (m *ShipmentDeliveredResponse) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_ShipmentDeliveredResponse.Unmarshal(m, b)
}
func (m *ShipmentDeliveredResponse) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_ShipmentDeliveredResponse.Marshal(b, m, deterministic)
}
func (m *ShipmentDeliveredResponse) XXX_Merge(src proto.Message) {
	xxx_messageInfo_ShipmentDeliveredResponse.Merge(m, src)
}
func (m *ShipmentDeliveredResponse) XXX_Size() int {
	return xxx_messageInfo_ShipmentDeliveredResponse.Size(m)
}
func (m *ShipmentDeliveredResponse) XXX_DiscardUnknown() {
	xxx_messageInfo_ShipmentDeliveredResponse.DiscardUnknown(m)
}

var xxx_messageInfo_ShipmentDeliveredResponse proto.InternalMessageInfo

func (m *ShipmentDeliveredResponse) GetShipmentId() string {
	if m != nil {
		return m.ShipmentId
	}
	return ""
}

func (m *ShipmentDeliveredResponse) GetExtraFees() int32 {
	if m != nil {
		return m.ExtraFees
	}
	return 0
}

func (m *ShipmentDeliveredResponse) GetSuccess() bool {
	if m != nil {
		return m.Success
	}
	return false
}

func init() {
	proto.RegisterType((*ShipmentDeliveredResponse)(nil), "BeeGees.Commands.Responses.ShipmentDeliveredResponse")
}

func init() { proto.RegisterFile("ShipmentDeliveredResponse.proto", fileDescriptor_7d3a34b25f17d508) }

var fileDescriptor_7d3a34b25f17d508 = []byte{
	// 149 bytes of a gzipped FileDescriptorProto
	0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0xe2, 0x92, 0x0f, 0xce, 0xc8, 0x2c,
	0xc8, 0x4d, 0xcd, 0x2b, 0x71, 0x49, 0xcd, 0xc9, 0x2c, 0x4b, 0x2d, 0x4a, 0x4d, 0x09, 0x4a, 0x2d,
	0x2e, 0xc8, 0xcf, 0x2b, 0x4e, 0xd5, 0x2b, 0x28, 0xca, 0x2f, 0xc9, 0x17, 0x92, 0x72, 0x4a, 0x4d,
	0x75, 0x4f, 0x4d, 0x2d, 0xd6, 0x73, 0xce, 0xcf, 0xcd, 0x4d, 0xcc, 0x4b, 0x29, 0xd6, 0x83, 0x29,
	0x28, 0x56, 0x2a, 0xe6, 0x92, 0xc4, 0xa9, 0x5d, 0x48, 0x8e, 0x8b, 0x0b, 0x26, 0xe9, 0x99, 0x22,
	0xc1, 0xa8, 0xc0, 0xa8, 0xc1, 0x19, 0x84, 0x24, 0x22, 0x24, 0xc3, 0xc5, 0xe9, 0x5a, 0x51, 0x52,
	0x94, 0xe8, 0x96, 0x9a, 0x5a, 0x2c, 0xc1, 0xa4, 0xc0, 0xa8, 0xc1, 0x1a, 0x84, 0x10, 0x10, 0x92,
	0xe0, 0x62, 0x0f, 0x2e, 0x4d, 0x4e, 0x4e, 0x2d, 0x2e, 0x96, 0x60, 0x56, 0x60, 0xd4, 0xe0, 0x08,
	0x82, 0x71, 0x93, 0xd8, 0xc0, 0xee, 0x32, 0x06, 0x04, 0x00, 0x00, 0xff, 0xff, 0xa8, 0xcc, 0x7f,
	0xab, 0xba, 0x00, 0x00, 0x00,
}
