//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/Login.proto
namespace protos.Login
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ReqLogin")]
  public partial class ReqLogin : global::ProtoBuf.IExtensible
  {
    public ReqLogin() {}
    
    private string _account = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string account
    {
      get { return _account; }
      set { _account = value; }
    }
    private string _password = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"password", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string password
    {
      get { return _password; }
      set { _password = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ReqCreateAccount")]
  public partial class ReqCreateAccount : global::ProtoBuf.IExtensible
  {
    public ReqCreateAccount() {}
    
    private string _account = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string account
    {
      get { return _account; }
      set { _account = value; }
    }
    private string _password = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"password", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string password
    {
      get { return _password; }
      set { _password = value; }
    }
    private string _name = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private string _role = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"role", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string role
    {
      get { return _role; }
      set { _role = value; }
    }
    private string _headImgStr = "";
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"headImgStr", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string headImgStr
    {
      get { return _headImgStr; }
      set { _headImgStr = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}