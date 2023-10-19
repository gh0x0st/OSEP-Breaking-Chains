#!/bin/bash

# Define file paths
cert="$HOME/PEN-300/Network Filters/cert.crt"
priv_key="$HOME/PEN-300/Network Filters/priv.key"
pem="$HOME/PEN-300/Network Filters/microsoft.pem"

# Generate the certificate and key with Microsoft details
openssl req -new -x509 -nodes -out "$cert" -keyout "$priv_key" -subj "/C=US/ST=Washington/L=Redmond/O=Microsoft/OU=Azure/CN=microsoft.com/emailAddress=info@microsoft.com"

# Combine the certificate and private key into a PEM file for msfconsole
cat "$priv_key" "$cert" > "$pem"

# Clean up
rm "$cert" "$priv_key"
