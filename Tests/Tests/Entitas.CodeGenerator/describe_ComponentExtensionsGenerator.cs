﻿using NSpec;
using Entitas.CodeGenerator;
using System;
using My.Namespace;
using System.Linq;

class describe_ComponentExtensionsGenerator : nspec {

    bool logResults = false;

    const string classSuffix = "GeneratedExtension";

    void generates(Type type, string code) {
        var files = new ComponentExtensionsGenerator().Generate(new[] { type });
        var filePath = type + classSuffix;

        files.Length.should_be(1);
        files.Any(f => f.fileName == filePath).should_be_true();

        var file = files.First(f => f.fileName == filePath);
        if (logResults) {
            Console.WriteLine("should:\n" + code);
            Console.WriteLine("was:\n" + file.fileContent);
        }
        file.fileContent.should_be(code);
    }

    void when_generating() {
        it["component without fields"] = () => generates(typeof(MovableComponent), MovableComponent.extensions);
        it["component with fields"] = () => generates(typeof(PersonComponent), PersonComponent.extensions);
        it["single singleton component"] = () => generates(typeof(AnimatingComponent), AnimatingComponent.extensions);
        it["single component with fields"] = () => generates(typeof(UserComponent), UserComponent.extensions);
        it["component for custom pool"] = () => generates(typeof(OtherPoolComponent), OtherPoolComponent.extensions);
        it["ignores [DontGenerate]"] = () => {
            var type = typeof(DontGenerateComponent);
            var files = new ComponentExtensionsGenerator().Generate(new[] { type });
            files.Length.should_be(0);
        };

        it["works with namespaces"] = () => generates(typeof(NamespaceComponent), NamespaceComponent.extensions);
    }
}

