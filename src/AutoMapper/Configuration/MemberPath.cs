﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoMapper.Internal
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct MemberPath : IEquatable<MemberPath>
    {
        public static readonly MemberPath Empty = new MemberPath(Array.Empty<MemberInfo>());
        public readonly MemberInfo[] Members;
        public MemberPath(Stack<Member> members) : this(members.ToMemberInfos())
        {
        }

        public MemberPath(MemberInfo[] members) => Members = members;

        public MemberInfo Last => Members[Members.Length - 1];

        public MemberInfo First => Members[0];

        public int Length => Members.Length;

        public bool Equals(MemberPath other) => Members.SequenceEqual(other.Members);

        public override bool Equals(object obj)
        {
            if(obj is null) return false;
            return obj is MemberPath path && Equals(path);
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            foreach(var member in Members)
            {
                hashCode = HashCodeCombiner.CombineCodes(hashCode, member.GetHashCode());
            }
            return hashCode;
        }

        public override string ToString()
            => string.Join(".", Members.Select(mi => mi.Name));

        public static bool operator==(in MemberPath left, in MemberPath right) => left.Equals(right);

        public static bool operator!=(in MemberPath left, in MemberPath right) => !left.Equals(right);

        public bool StartsWith(in MemberPath path)
        {
            if (path.Length > Length)
            {
                return false;
            }
            for (int index = 0; index < path.Length; index++)
            {
                if (Members[index] != path.Members[index])
                {
                    return false;
                }
            }
            return true;
        }

        public MemberPath Concat(IEnumerable<MemberInfo> memberInfos) => new MemberPath(Members.Concat(memberInfos).ToArray());
    }
}