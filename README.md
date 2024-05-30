## Entropy Compiler (`entc`)

This is the updated and improved Entropy compiler, bringing the intriguing world of Daniel Temkin's Esolang to modern development environments. Entropy is a unique programming language where data is in a constant state of decay. It challenges programmers to embrace impermanence, forcing them to convey their ideas before their programs inevitably crumble. 

**A 2024 Revival of a 2014 Classic**

This project revitalizes the original Entropy compiler, introducing:

- **Modern Command-Line Interface:**  Manage compilation and execution seamlessly with commands like `entc run`, `entc compile`, and more.
- **Enhanced Options:** Fine-tune your Entropy experience with options like `--mutation-rate` to control the rate of data decay.
- **Robust Error Handling:**  Write Entropy code with confidence, guided by clear and helpful error messages.
- **Cross-Platform Compatibility:** Developed using the Roslyn compiler platform, ensuring compatibility beyond the limitations of the original .NET Framework implementation.

### Quick Start

It's as simple as 1 and 2.

1. **Download:** Download the latest release of `entc`. 
2. **Run Your Entropy Code:**
    ```bash
    entc run my_entropy_program.en
    ```

### Installation

//to be written

### Commands and Options

- **`entc run <input_file.en>`:** Translates and executes your Entropy program.
- **`entc compile <input_file.en>`:**  Translates your Entropy program to C# and prints the output. 
- **`entc help`:** Displays a help message.

**Options:**

- **`-m=<value>`, `--mutation-rate=<value>`:** Controls the rate of data decay in your Entropy program. (Default: 2).
   - Values must be between 0.001 and 100.

**Example:**

```bash
entc run my_entropy_program.en --mutation-rate=0.5
```

### Contributions

Contributions, bug reports, and feature requests are welcome! Please open an issue or submit a pull request on the GitHub repository. 

## ☕ Buy me a coffee
If you use and enjoy lexido, you can buy me a coffee as a thank you!
https://ko-fi.com/micr0byte

<a href='https://ko-fi.com/J3J745R96' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://storage.ko-fi.com/cdn/kofi3.png?v=3' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>

### Resources

- **Original Entropy Language:**  Explore the original Entropy language website: http://danieltemkin.com/Entropy
- **Esolang Wiki:** Learn more about Esoteric Programming Languages (Esolangs) and Entropy: http://esolangs.org/wiki/Entropy
- **Drunk Eliza:** Witness Entropy in action with the mesmerizing "Drunk Eliza" project: http://danieltemkin.com/DrunkEliza

Made with 💚 by Micr0byte