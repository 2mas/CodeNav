﻿using CodeNav.Helpers;
using CodeNav.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Windows.Media;
using VisualBasicSyntax = Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace CodeNav.Mappers
{
    public static class StructMapper
    {
        public static CodeClassItem MapStruct(StructDeclarationSyntax member, 
            CodeViewUserControl control, SemanticModel semanticModel)
        {
            if (member == null) return null;

            var item = BaseMapper.MapBase<CodeClassItem>(member, member.Identifier, member.Modifiers, control, semanticModel);
            item.Kind = CodeItemKindEnum.Struct;
            item.Moniker = IconMapper.MapMoniker(item.Kind, item.Access);
            item.BorderColor = Colors.DarkGray;

            foreach (var structMember in member.Members)
            {
                item.Members.Add(SyntaxMapper.MapMember(structMember));
            }

            return item;
        }

        public static CodeClassItem MapStruct(VisualBasicSyntax.StructureBlockSyntax member,
            CodeViewUserControl control, SemanticModel semanticModel)
        {
            if (member == null) return null;

            var item = BaseMapper.MapBase<CodeClassItem>(member, member.StructureStatement.Identifier, 
                member.StructureStatement.Modifiers, control, semanticModel);
            item.Kind = CodeItemKindEnum.Struct;
            item.Moniker = IconMapper.MapMoniker(item.Kind, item.Access);
            item.BorderColor = Colors.DarkGray;

            foreach (var structMember in member.Members)
            {
                item.Members.Add(SyntaxMapper.MapMember(structMember));
            }

            return item;
        }
    }
}
