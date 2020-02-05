// Code generated by protoc-gen-go. DO NOT EDIT.
// source: GetAllShipmentsQuery.proto

package BeeGees_Queries

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

type GetAllShipmentsQuery struct {
	CustomerId           int32    `protobuf:"varint,1,opt,name=CustomerId,proto3" json:"CustomerId,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *GetAllShipmentsQuery) Reset()         { *m = GetAllShipmentsQuery{} }
func (m *GetAllShipmentsQuery) String() string { return proto.CompactTextString(m) }
func (*GetAllShipmentsQuery) ProtoMessage()    {}
func (*GetAllShipmentsQuery) Descriptor() ([]byte, []int) {
	return fileDescriptor_61ee92dae6cbbb29, []int{0}
}

func (m *GetAllShipmentsQuery) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_GetAllShipmentsQuery.Unmarshal(m, b)
}
func (m *GetAllShipmentsQuery) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_GetAllShipmentsQuery.Marshal(b, m, deterministic)
}
func (m *GetAllShipmentsQuery) XXX_Merge(src proto.Message) {
	xxx_messageInfo_GetAllShipmentsQuery.Merge(m, src)
}
func (m *GetAllShipmentsQuery) XXX_Size() int {
	return xxx_messageInfo_GetAllShipmentsQuery.Size(m)
}
func (m *GetAllShipmentsQuery) XXX_DiscardUnknown() {
	xxx_messageInfo_GetAllShipmentsQuery.DiscardUnknown(m)
}

var xxx_messageInfo_GetAllShipmentsQuery proto.InternalMessageInfo

func (m *GetAllShipmentsQuery) GetCustomerId() int32 {
	if m != nil {
		return m.CustomerId
	}
	return 0
}

func init() {
	proto.RegisterType((*GetAllShipmentsQuery)(nil), "BeeGees.Queries.GetAllShipmentsQuery")
}

func init() { proto.RegisterFile("GetAllShipmentsQuery.proto", fileDescriptor_61ee92dae6cbbb29) }

var fileDescriptor_61ee92dae6cbbb29 = []byte{
	// 101 bytes of a gzipped FileDescriptorProto
	0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0xe2, 0x92, 0x72, 0x4f, 0x2d, 0x71,
	0xcc, 0xc9, 0x09, 0xce, 0xc8, 0x2c, 0xc8, 0x4d, 0xcd, 0x2b, 0x29, 0x0e, 0x2c, 0x4d, 0x2d, 0xaa,
	0xd4, 0x2b, 0x28, 0xca, 0x2f, 0xc9, 0x17, 0xe2, 0x77, 0x4a, 0x4d, 0x75, 0x4f, 0x4d, 0x2d, 0xd6,
	0x03, 0x09, 0x66, 0xa6, 0x16, 0x2b, 0x99, 0x71, 0x89, 0x60, 0x53, 0x2e, 0x24, 0xc7, 0xc5, 0xe5,
	0x5c, 0x5a, 0x5c, 0x92, 0x9f, 0x9b, 0x5a, 0xe4, 0x99, 0x22, 0xc1, 0xa8, 0xc0, 0xa8, 0xc1, 0x1a,
	0x84, 0x24, 0x92, 0xc4, 0x06, 0x36, 0xcf, 0x18, 0x10, 0x00, 0x00, 0xff, 0xff, 0x86, 0x8c, 0xf9,
	0x53, 0x6d, 0x00, 0x00, 0x00,
}