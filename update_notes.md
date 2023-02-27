There are several "cleanups" changes. Wherever possible I tried not to let 
these spiral and kept them to self-contained usage issues, clarity, C# 10 style, and bugs. 
For example,

* Removed superfluous test file.
* Structured-logging calls.
* Updated nuget references.
* Remove unused `using` directives when working in a given code file.

There are some other minor refactorings which would be more involved but I avoided for time: 

* Use `async`/`await` everywhere; no blocking on asynchronous code.
* Using FluentAssertions for testing
* Refactor seeding (async startup process).
    * Register `EmployeeDataSeeder` with DI.
    * Use Async `Main()`.
        * after config, open DI scope and get the data-seeder.
        * `await` seeding after configuration.
        * use `RunAsync()`
    * Refactor JSON seed file 
        * EF can build a flat set internally from a tree.
        * Removes need for `FixUpReferences`
* Json.NET → System.Text.Json everywhere.

Other major refactorings what could be undertaken:

* Logging middleware.
* Error catching middleware.
* Sort functionality/interfaces into "Domain" and "Infrastructure" namespaces/subprojects for clarity.
* Adding unit tests.
* Adding fully separated input, output and data models, using a mapping tool (e.g., AutoMapper) for transformations.
    * This would be required to fix the apparent discrepancy between the existing output and
        model described in the documentation.

## Task 1 Notes
Introduced a simple iterative query mechanism. An "including children" query was added separately, so as
not to change the existing functionality (since the output is tied directly to the data model). It did
require changing the PUT to a more strict "update" model, since the existing "replace" model was damaging
the hierarchy and causing side-effects from the existing integration test.

A more complete solution would require a change to the data model. Most promising would be taking advantage
of the EF `HierarchyId`.