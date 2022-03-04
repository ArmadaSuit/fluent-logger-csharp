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

using System;
using System.Runtime.CompilerServices;

namespace Pigeon
{
    internal static class Extensions
    {
        // References
        // https://github.com/neuecc/MessagePack-CSharp/blob/v2.3.85/src/MessagePack.UnityClient/Assets/Scripts/MessagePack/Internal/ReflectionExtensions.cs#L23-L31
        public static bool IsAnonymous(this System.Reflection.TypeInfo type)
        {
            return type.Namespace == null
                   && type.IsSealed
                   && (type.Name.StartsWith("<>f__AnonymousType", StringComparison.Ordinal)
                       || type.Name.StartsWith("<>__AnonType", StringComparison.Ordinal)
                       || type.Name.StartsWith("VB$AnonymousType_", StringComparison.Ordinal))
                   && type.IsDefined(typeof(CompilerGeneratedAttribute), false);
        }
    }
}
