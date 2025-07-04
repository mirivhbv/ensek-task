TODO: add readme content
TODO: add .devcontainer

- talk about due to simplicity and given time range I didn't apply any dependency (assembly level) segragation applying stairway pattern or so. could be done by moving interface(s) to its own lib and making sure none interface has direct reference to 3rd party lib -- and 3rd party lib ref concrete assemblies would apply adapter pattern to role bridge.