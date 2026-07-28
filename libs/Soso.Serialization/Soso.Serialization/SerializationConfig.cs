using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Soso.Serialization.Reflection;
using Soso.Serialization.Serializers;
using Soso.Serialization.Serializers.Defaults;

namespace Soso.Serialization
{
	public class SerializationConfig
	{
		private readonly TypeFactory _factory;
		private readonly InterfaceMap _interfaceMap;
		private readonly SerializerMap _serializerMap;
		public static SerializationConfig Empty => new SerializationConfig();
		public static SerializationConfig Default => Empty.AddDefaultConfiguration();
		private SerializationConfig()
		{
			_factory = new TypeFactory();
			_interfaceMap = new InterfaceMap();
			_serializerMap = new SerializerMap();
		}

		/// <summary>
		/// Add a custom serializer
		/// </summary>
		/// <param name="serializer"></param>
		/// <typeparam name="T"></typeparam>
		public SerializationConfig AddSerializer<T>(ISerializer<T> serializer)
		{
			_serializerMap.AddSerializer(serializer);
			AddStreamingType<T>();
			return this;
		}

		/// <summary>
		/// Add a custom serializer for all given types
		/// </summary>
		/// <param name="serializer"></param>
		/// <param name="type"></param>
		/// <param name="types"></param>
		public SerializationConfig AddSerializer(ISerializer serializer, Type type, params Type[] types)
		{
			_serializerMap.AddSerializer(serializer, type, types);
			AddStreamingType(type);
			foreach (Type extra in types)
			{
				AddStreamingType(extra);
			}
			return this;
		}

		/// <summary>
		/// Add a custom serializer
		/// </summary>
		/// <param name="serialize"></param>
		/// <param name="deserialize"></param>
		/// <typeparam name="T"></typeparam>
		public SerializationConfig AddSerializer<T>(Serializer<T>.SerializeDelegate serialize, Serializer<T>.DeserializeDelegate deserialize, params Type[] additionTypes)
		{
			_serializerMap.AddSerializer(serialize, deserialize, additionTypes);
			AddStreamingType<T>();
			foreach (Type extra in additionTypes)
			{
				AddStreamingType(extra);
			}
			return this;
		}

		public SerializationConfig AddDefaultConfiguration()
		{
			AddSerializer(new ByteSerializer());
			AddSerializer(new SByteSerializer());
			AddSerializer(new ShortSerializer());
			AddSerializer(new UShortSerializer());
			AddSerializer(new IntSerializer());
			AddSerializer(new UIntSerializer());
			AddSerializer(new LongSerializer());
			AddSerializer(new ULongSerializer());
			AddSerializer(new FloatSerializer());
			AddSerializer(new DoubleSerializer());
			AddSerializer(new DecimalSerializer());
			AddSerializer(new CharSerializer());
			AddSerializer(new BoolSerializer());

			AddSerializer(new StringSerializer());
			AddSerializer(new DateTimeSerializer());
			SetFactory<DateTime>((args) => new DateTime());
			AddSerializer(new ArraySerializer());
			AddSerializer(new ListSerializer(typeof(List<>)), typeof(List<>), typeof(IList<>));
			AddSerializer(new ListSerializer(typeof(Collection<>)), typeof(Collection<>));
			AddSerializer(new DictionarySerializer(), typeof(Dictionary<,>));

			return this;
		}

		/// <summary>
		/// Register an interface for a type
		/// </summary>
		/// <typeparam name="TI"></typeparam>
		/// <typeparam name="T"></typeparam>
		public SerializationConfig AddMapping<TI, T>()
		{
			_interfaceMap.AddMapping<TI, T>();
			AddStreamingType<TI>();
			AddStreamingType<T>();
			return this;
		}

		/// <summary>
		/// Register a method to create an instance faster
		/// </summary>
		/// <param name="create"></param>
		/// <typeparam name="T"></typeparam>
		public SerializationConfig SetFactory<T>(Func<object[], object> create)
		{
			_factory.SetFactory<T>(create);
			AddStreamingType<T>();
			return this;
		}

		/// <summary>
		/// Add available type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public SerializationConfig AddStreamingType<T>()
		{
			SosoTypeCode.AddStreamingType(typeof(T));
			return this;
		}

		/// <summary>
		/// Add available type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public SerializationConfig AddStreamingType(Type type)
		{
			TypeMemberCache.Cache(type);
			SosoTypeCode.AddStreamingType(type);
			return this;
		}

		public TypeFactory GetFactory()
		{
			return _factory;
		}

		public InterfaceMap GetInterfaceMapping()
		{
			return _interfaceMap;
		}

		public SerializerMap GetSerializers()
		{
			return _serializerMap;
		}
	}
}
