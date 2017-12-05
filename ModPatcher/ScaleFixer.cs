using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Meta;
using UnityEngine;
public class ScaleFixer:MonoBehaviour
    {
    public Vector3 scale = Vector3.one;
    Vector3 AdjustedScale = Vector3.zero;
        public void Awake()
        {
            gameObject.transform.localScale = scale;
        }
        public void Start()
        {
            gameObject.transform.localScale = scale;
            
        }
    public void OnDropped(Pickupable pickupable)
    {
        if (gameObject.transform.parent == null)
        {

            if (gameObject.transform.localScale != scale)
            {
                gameObject.transform.localScale = scale;
            }
        }
        else
        {
            var parent = gameObject.transform.parent;
            AdjustedScale.x = scale.x / parent.localScale.x;
            AdjustedScale.y = scale.y / parent.localScale.y;
            AdjustedScale.z = scale.z / parent.localScale.z;
            if (gameObject.transform.localScale != AdjustedScale)
            {
                gameObject.transform.localScale = AdjustedScale;
            }
        }
    }
    public void UpdateAdjustedScale()
    {
        var parent = gameObject.transform.parent;
        AdjustedScale.x = scale.x / parent.localScale.x;
        AdjustedScale.y = scale.y / parent.localScale.y;
        AdjustedScale.z = scale.z / parent.localScale.z;
        parent = parent.parent;
        while (parent != null)
        {
            AdjustedScale.x = AdjustedScale.x / parent.localScale.x;
            AdjustedScale.y = AdjustedScale.y / parent.localScale.y;
            AdjustedScale.z = AdjustedScale.z / parent.localScale.z;
            parent = parent.parent;
        }
    }
    public void Update()
    {
        if (gameObject.transform.parent==null)
        {
            
            if (gameObject.transform.localScale != scale)
            {

                gameObject.transform.localScale = scale;
            }
        }
        else
        {
            UpdateAdjustedScale();
            if (gameObject.transform.localScale != AdjustedScale)
            {

                gameObject.transform.localScale = AdjustedScale;
            }
        }
    }
    
}
public class ScaleFixerSerializer : ICustomSerializer
{
    public object Deserialize(object obj, ProtoReader reader, ProtobufSerializerPrecompiled model)
    {
        var scale = obj as ScaleFixer;
        reader.ReadFieldHeader();
        var token = ProtoReader.StartSubItem(reader);
        scale.scale = (Vector3)AlternativeSerializer.PrecompDeserialize(model, AlternativeSerializer.PrecompGetKey(model, typeof(Vector3)),  scale.scale, reader);
        ProtoReader.EndSubItem(token, reader);
        return scale;
    }

    public void Serialize(object obj, ProtoWriter writer, ProtobufSerializerPrecompiled model)
    {
        var scale = obj as ScaleFixer;
        ProtoWriter.WriteFieldHeader(1, WireType.String, writer);
        var token = ProtoWriter.StartSubItem(null, writer);
        AlternativeSerializer.PrecompSerialize(model, AlternativeSerializer.PrecompGetKey(model, typeof(Vector3)), scale.scale, writer);
        ProtoWriter.EndSubItem(token, writer);

    }
}

