#!/bin/bash

# Set the output directory for the binaries
OUTPUT_DIR="binaries"

# Create the output directory if it doesn't exist
mkdir -p "${OUTPUT_DIR}"

# Function to build the project for a specific runtime
build_for_runtime() {
    local runtime="$1"
    local output_folder="${OUTPUT_DIR}/${runtime}"

    echo "Building for runtime: ${runtime}"

    dotnet publish -c Release -r "${runtime}" -o "${output_folder}"
    if [ $? -ne 0 ]; then
        echo "Build failed for runtime: ${runtime}"
        exit 1
    fi

    echo "Build successful for runtime: ${runtime}"
}

# Build for all specified runtimes
build_for_runtime "linux-x64"
build_for_runtime "linux-arm64"
build_for_runtime "osx-x64"
build_for_runtime "osx-arm64"
build_for_runtime "win-x86"
build_for_runtime "win-arm64"

echo "All builds completed!"