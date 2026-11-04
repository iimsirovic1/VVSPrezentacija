
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
    call.getTarget().hasQualifiedName("System.Security.Cryptography.MD5.HashData") or
    call.getTarget().hasQualifiedName("System.Security.Cryptography.MD5.Create")
  ) and algorithm = "MD5"
  or
  (
    call.getTarget().hasQualifiedName("System.Security.Cryptography.SHA1.HashData") or
    call.getTarget().hasQualifiedName("System.Security.Cryptography.SHA1.Create")
  ) and algorithm = "SHA-1"
  or
  call.getTarget().hasQualifiedName("System.Security.Cryptography.DES.Create")
  and algorithm = "DES"
select call, "Detected use of insecure cryptographic algorithm: " + algorithm + "."
