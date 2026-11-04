
/**
 * @name Weak cryptographic algorithms
 * @description Detects MD5, SHA-1 and DES in the demo.
 * @kind problem
 * @problem.severity warning
 * @security-severity 7.0
 * @precision high
 * @id cs/demo-weak-crypto
 * @tags security
 *       external/cwe/cwe-327
 *       external/cwe/cwe-328
 */

import csharp

from MethodCall call, string algorithm
where
  (
    call.getTarget().getDeclaringType().hasQualifiedName("System.Security.Cryptography.MD5") and
    (
      call.getTarget().hasName("HashData") or
      call.getTarget().hasName("Create")
    ) and
    algorithm = "MD5"
  )
  or
  (
    call.getTarget().getDeclaringType().hasQualifiedName("System.Security.Cryptography.SHA1") and
    (
      call.getTarget().hasName("HashData") or
      call.getTarget().hasName("Create")
    ) and
    algorithm = "SHA-1"
  )
  or
  (
    call.getTarget().getDeclaringType().hasQualifiedName("System.Security.Cryptography.DES") and
    call.getTarget().hasName("Create") and
    algorithm = "DES"
  )
select call, "Detected use of insecure cryptographic algorithm: " + algorithm + "."
