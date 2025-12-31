#!/usr/bin/env python3
"""
Script to generate Python protobuf files from .proto definitions.
This script references the proto files in the main project to avoid duplication.
Works in both local development and Docker build contexts.
"""

import os
import sys
from pathlib import Path
from grpc_tools import protoc

# Paths - handle both local and Docker contexts
if Path("/proto_source/BeeGees_Messaging").exists():
    # Docker context
    PROTO_SOURCE_DIR = Path("/proto_source/BeeGees_Messaging")
    OUTPUT_DIR = Path("/app/app/proto_generated")
else:
    # Local development context
    PROJECT_ROOT = Path(__file__).parent.parent
    PROTO_SOURCE_DIR = PROJECT_ROOT / "src" / "BeeGees_Messaging"
    OUTPUT_DIR = Path(__file__).parent / "app" / "proto_generated"

def generate_proto_files():
    """Generate Python code from protobuf definitions"""

    # Create output directory
    OUTPUT_DIR.mkdir(exist_ok=True)

    # Create __init__.py files
    (OUTPUT_DIR / "__init__.py").write_text("")

    # Find all .proto files
    proto_files = list(PROTO_SOURCE_DIR.rglob("*.proto"))

    if not proto_files:
        print(f"No .proto files found in {PROTO_SOURCE_DIR}")
        return

    print(f"Found {len(proto_files)} proto files")
    print(f"Generating Python code to {OUTPUT_DIR}...")

    # Generate Python code for all proto files at once
    proto_paths = [str(proto_file.relative_to(PROTO_SOURCE_DIR)) for proto_file in proto_files]

    # Build protoc command arguments
    args = [
        'grpc_tools.protoc',
        f'--proto_path={PROTO_SOURCE_DIR}',
        f'--python_out={OUTPUT_DIR}',
    ] + proto_paths

    print(f"Running protoc...")
    result = protoc.main(args)

    if result != 0:
        print(f"Error generating proto files (exit code: {result})")
        sys.exit(1)

    # Create __init__.py in subdirectories
    for subdir in OUTPUT_DIR.rglob("*/"):
        init_file = subdir / "__init__.py"
        if not init_file.exists():
            init_file.write_text("")

    print("✓ Proto generation complete!")
    print(f"Python files generated in: {OUTPUT_DIR}")

    # List generated files
    print("\nGenerated files:")
    for py_file in sorted(OUTPUT_DIR.rglob("*.py")):
        if py_file.name != "__init__.py":
            print(f"  - {py_file.relative_to(OUTPUT_DIR)}")


if __name__ == "__main__":
    generate_proto_files()
