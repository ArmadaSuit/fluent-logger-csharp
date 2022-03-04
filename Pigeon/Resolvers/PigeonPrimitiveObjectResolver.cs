// Pigeon
//
// Copyright 2022 ArmadaSuit and contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using MessagePack;
using MessagePack.Formatters;
using Pigeon.Formatters;

namespace Pigeon.Resolvers
{
    public sealed class PigeonPrimitiveObjectResolver : IFormatterResolver
    {
        /// <summary>
        /// The singleton instance that can be used.
        /// </summary>
        public static readonly PigeonPrimitiveObjectResolver Instance;

        /// <summary>
        /// A <see cref="MessagePackSerializerOptions"/> instance with this formatter pre-configured.
        /// </summary>
        public static readonly MessagePackSerializerOptions Options;

        static PigeonPrimitiveObjectResolver()
        {
            Instance = new PigeonPrimitiveObjectResolver();
            Options = MessagePackSerializerOptions.Standard.WithResolver(Instance);
        }

        private PigeonPrimitiveObjectResolver()
        {
        }

        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        private static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T> Formatter;

            private static readonly IMessagePackFormatter<object> ObjectFormatter =
                new PigeonPrimitiveObjectFormatter(DateAndTimeFormatters.DateTimeStringFormatter.Instance,
                    DateAndTimeFormatters.DateTimeOffsetStringFormatter.Instance);

            static FormatterCache()
            {
                Formatter = (typeof(T) == typeof(object))
                    ? (IMessagePackFormatter<T>) (object) ObjectFormatter
                    : null;
            }
        }
    }
}
