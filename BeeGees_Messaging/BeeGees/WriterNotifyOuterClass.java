// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: WriterNotify.proto

package BeeGees;

public final class WriterNotifyOuterClass {
  private WriterNotifyOuterClass() {}
  public static void registerAllExtensions(
      com.google.protobuf.ExtensionRegistryLite registry) {
  }

  public static void registerAllExtensions(
      com.google.protobuf.ExtensionRegistry registry) {
    registerAllExtensions(
        (com.google.protobuf.ExtensionRegistryLite) registry);
  }
  public interface WriterNotifyOrBuilder extends
      // @@protoc_insertion_point(interface_extends:BeeGees.WriterNotify)
      com.google.protobuf.MessageOrBuilder {

    /**
     * <code>int32 Type = 1;</code>
     * @return The type.
     */
    int getType();

    /**
     * <code>bytes Blob = 2;</code>
     * @return The blob.
     */
    com.google.protobuf.ByteString getBlob();
  }
  /**
   * Protobuf type {@code BeeGees.WriterNotify}
   */
  public  static final class WriterNotify extends
      com.google.protobuf.GeneratedMessageV3 implements
      // @@protoc_insertion_point(message_implements:BeeGees.WriterNotify)
      WriterNotifyOrBuilder {
  private static final long serialVersionUID = 0L;
    // Use WriterNotify.newBuilder() to construct.
    private WriterNotify(com.google.protobuf.GeneratedMessageV3.Builder<?> builder) {
      super(builder);
    }
    private WriterNotify() {
      blob_ = com.google.protobuf.ByteString.EMPTY;
    }

    @java.lang.Override
    @SuppressWarnings({"unused"})
    protected java.lang.Object newInstance(
        UnusedPrivateParameter unused) {
      return new WriterNotify();
    }

    @java.lang.Override
    public final com.google.protobuf.UnknownFieldSet
    getUnknownFields() {
      return this.unknownFields;
    }
    private WriterNotify(
        com.google.protobuf.CodedInputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      this();
      if (extensionRegistry == null) {
        throw new java.lang.NullPointerException();
      }
      com.google.protobuf.UnknownFieldSet.Builder unknownFields =
          com.google.protobuf.UnknownFieldSet.newBuilder();
      try {
        boolean done = false;
        while (!done) {
          int tag = input.readTag();
          switch (tag) {
            case 0:
              done = true;
              break;
            case 8: {

              type_ = input.readInt32();
              break;
            }
            case 18: {

              blob_ = input.readBytes();
              break;
            }
            default: {
              if (!parseUnknownField(
                  input, unknownFields, extensionRegistry, tag)) {
                done = true;
              }
              break;
            }
          }
        }
      } catch (com.google.protobuf.InvalidProtocolBufferException e) {
        throw e.setUnfinishedMessage(this);
      } catch (java.io.IOException e) {
        throw new com.google.protobuf.InvalidProtocolBufferException(
            e).setUnfinishedMessage(this);
      } finally {
        this.unknownFields = unknownFields.build();
        makeExtensionsImmutable();
      }
    }
    public static final com.google.protobuf.Descriptors.Descriptor
        getDescriptor() {
      return BeeGees.WriterNotifyOuterClass.internal_static_BeeGees_WriterNotify_descriptor;
    }

    @java.lang.Override
    protected com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
        internalGetFieldAccessorTable() {
      return BeeGees.WriterNotifyOuterClass.internal_static_BeeGees_WriterNotify_fieldAccessorTable
          .ensureFieldAccessorsInitialized(
              BeeGees.WriterNotifyOuterClass.WriterNotify.class, BeeGees.WriterNotifyOuterClass.WriterNotify.Builder.class);
    }

    public static final int TYPE_FIELD_NUMBER = 1;
    private int type_;
    /**
     * <code>int32 Type = 1;</code>
     * @return The type.
     */
    public int getType() {
      return type_;
    }

    public static final int BLOB_FIELD_NUMBER = 2;
    private com.google.protobuf.ByteString blob_;
    /**
     * <code>bytes Blob = 2;</code>
     * @return The blob.
     */
    public com.google.protobuf.ByteString getBlob() {
      return blob_;
    }

    private byte memoizedIsInitialized = -1;
    @java.lang.Override
    public final boolean isInitialized() {
      byte isInitialized = memoizedIsInitialized;
      if (isInitialized == 1) return true;
      if (isInitialized == 0) return false;

      memoizedIsInitialized = 1;
      return true;
    }

    @java.lang.Override
    public void writeTo(com.google.protobuf.CodedOutputStream output)
                        throws java.io.IOException {
      if (type_ != 0) {
        output.writeInt32(1, type_);
      }
      if (!blob_.isEmpty()) {
        output.writeBytes(2, blob_);
      }
      unknownFields.writeTo(output);
    }

    @java.lang.Override
    public int getSerializedSize() {
      int size = memoizedSize;
      if (size != -1) return size;

      size = 0;
      if (type_ != 0) {
        size += com.google.protobuf.CodedOutputStream
          .computeInt32Size(1, type_);
      }
      if (!blob_.isEmpty()) {
        size += com.google.protobuf.CodedOutputStream
          .computeBytesSize(2, blob_);
      }
      size += unknownFields.getSerializedSize();
      memoizedSize = size;
      return size;
    }

    @java.lang.Override
    public boolean equals(final java.lang.Object obj) {
      if (obj == this) {
       return true;
      }
      if (!(obj instanceof BeeGees.WriterNotifyOuterClass.WriterNotify)) {
        return super.equals(obj);
      }
      BeeGees.WriterNotifyOuterClass.WriterNotify other = (BeeGees.WriterNotifyOuterClass.WriterNotify) obj;

      if (getType()
          != other.getType()) return false;
      if (!getBlob()
          .equals(other.getBlob())) return false;
      if (!unknownFields.equals(other.unknownFields)) return false;
      return true;
    }

    @java.lang.Override
    public int hashCode() {
      if (memoizedHashCode != 0) {
        return memoizedHashCode;
      }
      int hash = 41;
      hash = (19 * hash) + getDescriptor().hashCode();
      hash = (37 * hash) + TYPE_FIELD_NUMBER;
      hash = (53 * hash) + getType();
      hash = (37 * hash) + BLOB_FIELD_NUMBER;
      hash = (53 * hash) + getBlob().hashCode();
      hash = (29 * hash) + unknownFields.hashCode();
      memoizedHashCode = hash;
      return hash;
    }

    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseFrom(
        java.nio.ByteBuffer data)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data);
    }
    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseFrom(
        java.nio.ByteBuffer data,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data, extensionRegistry);
    }
    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseFrom(
        com.google.protobuf.ByteString data)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data);
    }
    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseFrom(
        com.google.protobuf.ByteString data,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data, extensionRegistry);
    }
    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseFrom(byte[] data)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data);
    }
    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseFrom(
        byte[] data,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data, extensionRegistry);
    }
    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseFrom(java.io.InputStream input)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input);
    }
    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseFrom(
        java.io.InputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input, extensionRegistry);
    }
    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseDelimitedFrom(java.io.InputStream input)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseDelimitedWithIOException(PARSER, input);
    }
    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseDelimitedFrom(
        java.io.InputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseDelimitedWithIOException(PARSER, input, extensionRegistry);
    }
    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseFrom(
        com.google.protobuf.CodedInputStream input)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input);
    }
    public static BeeGees.WriterNotifyOuterClass.WriterNotify parseFrom(
        com.google.protobuf.CodedInputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input, extensionRegistry);
    }

    @java.lang.Override
    public Builder newBuilderForType() { return newBuilder(); }
    public static Builder newBuilder() {
      return DEFAULT_INSTANCE.toBuilder();
    }
    public static Builder newBuilder(BeeGees.WriterNotifyOuterClass.WriterNotify prototype) {
      return DEFAULT_INSTANCE.toBuilder().mergeFrom(prototype);
    }
    @java.lang.Override
    public Builder toBuilder() {
      return this == DEFAULT_INSTANCE
          ? new Builder() : new Builder().mergeFrom(this);
    }

    @java.lang.Override
    protected Builder newBuilderForType(
        com.google.protobuf.GeneratedMessageV3.BuilderParent parent) {
      Builder builder = new Builder(parent);
      return builder;
    }
    /**
     * Protobuf type {@code BeeGees.WriterNotify}
     */
    public static final class Builder extends
        com.google.protobuf.GeneratedMessageV3.Builder<Builder> implements
        // @@protoc_insertion_point(builder_implements:BeeGees.WriterNotify)
        BeeGees.WriterNotifyOuterClass.WriterNotifyOrBuilder {
      public static final com.google.protobuf.Descriptors.Descriptor
          getDescriptor() {
        return BeeGees.WriterNotifyOuterClass.internal_static_BeeGees_WriterNotify_descriptor;
      }

      @java.lang.Override
      protected com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
          internalGetFieldAccessorTable() {
        return BeeGees.WriterNotifyOuterClass.internal_static_BeeGees_WriterNotify_fieldAccessorTable
            .ensureFieldAccessorsInitialized(
                BeeGees.WriterNotifyOuterClass.WriterNotify.class, BeeGees.WriterNotifyOuterClass.WriterNotify.Builder.class);
      }

      // Construct using BeeGees.WriterNotifyOuterClass.WriterNotify.newBuilder()
      private Builder() {
        maybeForceBuilderInitialization();
      }

      private Builder(
          com.google.protobuf.GeneratedMessageV3.BuilderParent parent) {
        super(parent);
        maybeForceBuilderInitialization();
      }
      private void maybeForceBuilderInitialization() {
        if (com.google.protobuf.GeneratedMessageV3
                .alwaysUseFieldBuilders) {
        }
      }
      @java.lang.Override
      public Builder clear() {
        super.clear();
        type_ = 0;

        blob_ = com.google.protobuf.ByteString.EMPTY;

        return this;
      }

      @java.lang.Override
      public com.google.protobuf.Descriptors.Descriptor
          getDescriptorForType() {
        return BeeGees.WriterNotifyOuterClass.internal_static_BeeGees_WriterNotify_descriptor;
      }

      @java.lang.Override
      public BeeGees.WriterNotifyOuterClass.WriterNotify getDefaultInstanceForType() {
        return BeeGees.WriterNotifyOuterClass.WriterNotify.getDefaultInstance();
      }

      @java.lang.Override
      public BeeGees.WriterNotifyOuterClass.WriterNotify build() {
        BeeGees.WriterNotifyOuterClass.WriterNotify result = buildPartial();
        if (!result.isInitialized()) {
          throw newUninitializedMessageException(result);
        }
        return result;
      }

      @java.lang.Override
      public BeeGees.WriterNotifyOuterClass.WriterNotify buildPartial() {
        BeeGees.WriterNotifyOuterClass.WriterNotify result = new BeeGees.WriterNotifyOuterClass.WriterNotify(this);
        result.type_ = type_;
        result.blob_ = blob_;
        onBuilt();
        return result;
      }

      @java.lang.Override
      public Builder clone() {
        return super.clone();
      }
      @java.lang.Override
      public Builder setField(
          com.google.protobuf.Descriptors.FieldDescriptor field,
          java.lang.Object value) {
        return super.setField(field, value);
      }
      @java.lang.Override
      public Builder clearField(
          com.google.protobuf.Descriptors.FieldDescriptor field) {
        return super.clearField(field);
      }
      @java.lang.Override
      public Builder clearOneof(
          com.google.protobuf.Descriptors.OneofDescriptor oneof) {
        return super.clearOneof(oneof);
      }
      @java.lang.Override
      public Builder setRepeatedField(
          com.google.protobuf.Descriptors.FieldDescriptor field,
          int index, java.lang.Object value) {
        return super.setRepeatedField(field, index, value);
      }
      @java.lang.Override
      public Builder addRepeatedField(
          com.google.protobuf.Descriptors.FieldDescriptor field,
          java.lang.Object value) {
        return super.addRepeatedField(field, value);
      }
      @java.lang.Override
      public Builder mergeFrom(com.google.protobuf.Message other) {
        if (other instanceof BeeGees.WriterNotifyOuterClass.WriterNotify) {
          return mergeFrom((BeeGees.WriterNotifyOuterClass.WriterNotify)other);
        } else {
          super.mergeFrom(other);
          return this;
        }
      }

      public Builder mergeFrom(BeeGees.WriterNotifyOuterClass.WriterNotify other) {
        if (other == BeeGees.WriterNotifyOuterClass.WriterNotify.getDefaultInstance()) return this;
        if (other.getType() != 0) {
          setType(other.getType());
        }
        if (other.getBlob() != com.google.protobuf.ByteString.EMPTY) {
          setBlob(other.getBlob());
        }
        this.mergeUnknownFields(other.unknownFields);
        onChanged();
        return this;
      }

      @java.lang.Override
      public final boolean isInitialized() {
        return true;
      }

      @java.lang.Override
      public Builder mergeFrom(
          com.google.protobuf.CodedInputStream input,
          com.google.protobuf.ExtensionRegistryLite extensionRegistry)
          throws java.io.IOException {
        BeeGees.WriterNotifyOuterClass.WriterNotify parsedMessage = null;
        try {
          parsedMessage = PARSER.parsePartialFrom(input, extensionRegistry);
        } catch (com.google.protobuf.InvalidProtocolBufferException e) {
          parsedMessage = (BeeGees.WriterNotifyOuterClass.WriterNotify) e.getUnfinishedMessage();
          throw e.unwrapIOException();
        } finally {
          if (parsedMessage != null) {
            mergeFrom(parsedMessage);
          }
        }
        return this;
      }

      private int type_ ;
      /**
       * <code>int32 Type = 1;</code>
       * @return The type.
       */
      public int getType() {
        return type_;
      }
      /**
       * <code>int32 Type = 1;</code>
       * @param value The type to set.
       * @return This builder for chaining.
       */
      public Builder setType(int value) {
        
        type_ = value;
        onChanged();
        return this;
      }
      /**
       * <code>int32 Type = 1;</code>
       * @return This builder for chaining.
       */
      public Builder clearType() {
        
        type_ = 0;
        onChanged();
        return this;
      }

      private com.google.protobuf.ByteString blob_ = com.google.protobuf.ByteString.EMPTY;
      /**
       * <code>bytes Blob = 2;</code>
       * @return The blob.
       */
      public com.google.protobuf.ByteString getBlob() {
        return blob_;
      }
      /**
       * <code>bytes Blob = 2;</code>
       * @param value The blob to set.
       * @return This builder for chaining.
       */
      public Builder setBlob(com.google.protobuf.ByteString value) {
        if (value == null) {
    throw new NullPointerException();
  }
  
        blob_ = value;
        onChanged();
        return this;
      }
      /**
       * <code>bytes Blob = 2;</code>
       * @return This builder for chaining.
       */
      public Builder clearBlob() {
        
        blob_ = getDefaultInstance().getBlob();
        onChanged();
        return this;
      }
      @java.lang.Override
      public final Builder setUnknownFields(
          final com.google.protobuf.UnknownFieldSet unknownFields) {
        return super.setUnknownFields(unknownFields);
      }

      @java.lang.Override
      public final Builder mergeUnknownFields(
          final com.google.protobuf.UnknownFieldSet unknownFields) {
        return super.mergeUnknownFields(unknownFields);
      }


      // @@protoc_insertion_point(builder_scope:BeeGees.WriterNotify)
    }

    // @@protoc_insertion_point(class_scope:BeeGees.WriterNotify)
    private static final BeeGees.WriterNotifyOuterClass.WriterNotify DEFAULT_INSTANCE;
    static {
      DEFAULT_INSTANCE = new BeeGees.WriterNotifyOuterClass.WriterNotify();
    }

    public static BeeGees.WriterNotifyOuterClass.WriterNotify getDefaultInstance() {
      return DEFAULT_INSTANCE;
    }

    private static final com.google.protobuf.Parser<WriterNotify>
        PARSER = new com.google.protobuf.AbstractParser<WriterNotify>() {
      @java.lang.Override
      public WriterNotify parsePartialFrom(
          com.google.protobuf.CodedInputStream input,
          com.google.protobuf.ExtensionRegistryLite extensionRegistry)
          throws com.google.protobuf.InvalidProtocolBufferException {
        return new WriterNotify(input, extensionRegistry);
      }
    };

    public static com.google.protobuf.Parser<WriterNotify> parser() {
      return PARSER;
    }

    @java.lang.Override
    public com.google.protobuf.Parser<WriterNotify> getParserForType() {
      return PARSER;
    }

    @java.lang.Override
    public BeeGees.WriterNotifyOuterClass.WriterNotify getDefaultInstanceForType() {
      return DEFAULT_INSTANCE;
    }

  }

  private static final com.google.protobuf.Descriptors.Descriptor
    internal_static_BeeGees_WriterNotify_descriptor;
  private static final 
    com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
      internal_static_BeeGees_WriterNotify_fieldAccessorTable;

  public static com.google.protobuf.Descriptors.FileDescriptor
      getDescriptor() {
    return descriptor;
  }
  private static  com.google.protobuf.Descriptors.FileDescriptor
      descriptor;
  static {
    java.lang.String[] descriptorData = {
      "\n\022WriterNotify.proto\022\007BeeGees\"*\n\014WriterN" +
      "otify\022\014\n\004Type\030\001 \001(\005\022\014\n\004Blob\030\002 \001(\014b\006proto" +
      "3"
    };
    descriptor = com.google.protobuf.Descriptors.FileDescriptor
      .internalBuildGeneratedFileFrom(descriptorData,
        new com.google.protobuf.Descriptors.FileDescriptor[] {
        });
    internal_static_BeeGees_WriterNotify_descriptor =
      getDescriptor().getMessageTypes().get(0);
    internal_static_BeeGees_WriterNotify_fieldAccessorTable = new
      com.google.protobuf.GeneratedMessageV3.FieldAccessorTable(
        internal_static_BeeGees_WriterNotify_descriptor,
        new java.lang.String[] { "Type", "Blob", });
  }

  // @@protoc_insertion_point(outer_class_scope)
}
