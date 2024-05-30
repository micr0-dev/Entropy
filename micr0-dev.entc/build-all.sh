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

# Function to compress the binaries for a specific runtime
compress_binaries() {
    local runtime="$1"
    local output_folder="${OUTPUT_DIR}/${runtime}"
    local archive_name="${OUTPUT_DIR}/${runtime}"

    archive_name="${archive_name}.tar.gz"
    tar -czvf "${archive_name}" -C "${OUTPUT_DIR}" "${runtime}"

    # Remove the uncompressed binaries if the archive was created successfully
    if [ $? -eq 0 ]; then
        echo "Compressed binaries for ${runtime} to ${archive_name}"
        rm -rf "${output_folder}"
    else
        echo "Failed to compress binaries for ${runtime}"
    fi
}

# Build for all specified runtimes
build_for_runtime "linux-x64"
build_for_runtime "linux-arm64"
build_for_runtime "osx-x64"
build_for_runtime "osx-arm64"
build_for_runtime "win-x64"
build_for_runtime "win-arm64"

echo "All builds completed!"

# Compress binaries for all runtimes
compress_binaries "linux-x64"
compress_binaries "linux-arm64"
compress_binaries "osx-x64"
compress_binaries "osx-arm64"
compress_binaries "win-x64"
compress_binaries "win-arm64"

echo "All Compression completed!"
