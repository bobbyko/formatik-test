using Xunit;
using FluentAssertions;
using System.IO;
using System.Linq;
using System.Text;

namespace Octagon.Formatik.Tests
{
    public class Formatik_Tests
    {
        [Fact]
        public void json_list_1()
        {
            var input = File.ReadAllText("../../../TestData/1.json-list.1/input.json").Replace("\r", "");

            // validate Evaluation
            var formatik = new Formatik(
                input,
                File.ReadAllText("../../../TestData/1.json-list.1/example.txt").Replace("\r", ""));

            formatik.Should().NotBeNull();
            formatik.Header.Should().BeEmpty("Header should be empty");
            formatik.Footer.Should().BeEmpty("Footer should be empty");
            formatik.Separators.Should().NotBeNullOrEmpty("there should be separator");
            formatik.Separators.First().Should().Be("\n", "separator is a new line");
            formatik.Tokens.Should().NotBeNull("there should be tokens");
            formatik.Tokens.Count().Should().Be(1, "there should be one token");

            var token = formatik.Tokens.ElementAt(0);
            token.InputSelector.Should().Be("_id", "token is _id");
            token.OutputSelector.Should().Be("0", "output is a list of one token");
            token.Prefix.Should().BeEmpty("Token's prefix should be empty");
            token.Suffix.Should().BeEmpty("Token's suffix should be empty");

            // validate processing
            var processed = formatik.Process(input, Encoding.ASCII);
            var expectedOutput = File.ReadAllText("../../../TestData/1.json-list.1/output.txt");

            processed.Should().Be(expectedOutput.Replace("\r", ""), "processed output should be equal to expected output");
        }

        [Fact]
        public void json_list_2()
        {
            var input = File.ReadAllText("../../../TestData/1.json-list.2/input.json").Replace("\r", "");

            var formatik = new Formatik(
                input,
                File.ReadAllText("../../../TestData/1.json-list.2/example.txt").Replace("\r", ""));

            formatik.Should().NotBeNull();
            formatik.Header.Should().BeEmpty("Header should be empty");
            formatik.Footer.Should().BeEmpty("Footer should be empty");
            formatik.Separators.Should().NotBeNullOrEmpty("there should be separator");
            formatik.Separators.First().Should().Be("\n", "separator is a new line");
            formatik.Tokens.Should().NotBeNull("there should be tokens");
            formatik.Tokens.Count().Should().Be(1, "there should be one token");

            var token = formatik.Tokens.ElementAt(0);
            token.InputSelector.Should().Be("_date", "token is _date");
            token.OutputSelector.Should().Be("0", "output is a list of one token");
            token.Prefix.Should().BeEmpty("Token's prefix should be empty");
            token.Suffix.Should().BeEmpty("Token's suffix should be empty");

            // validate processing
            var processed = formatik.Process(input, Encoding.ASCII);
            var expectedOutput = File.ReadAllText("../../../TestData/1.json-list.2/output.txt");

            processed.Should().Be(expectedOutput.Replace("\r", ""), "processed output should be equal to expected output");
        }

        [Fact]
        public void json_list_3()
        {
            Assert.Throws<FormatikException>(() =>
            {
                var formatik = new Formatik(
                    File.ReadAllText("../../../TestData/1.json-list.3/input.json").Replace("\r", ""),
                    File.ReadAllText("../../../TestData/1.json-list.3/example.txt").Replace("\r", ""));
            });
        }

        [Fact]
        public void json_csv_1()
        {
            var input = File.ReadAllText("../../../TestData/2.json-csv.1/input.json").Replace("\r", "");

            var formatik = new Formatik(
                input,
                File.ReadAllText("../../../TestData/2.json-csv.1/example.csv").Replace("\r", ""));

            formatik.Should().NotBeNull();
            formatik.Header.Should().Be("id,timestamp,accountId,country\n");
            formatik.Footer.Should().Be("\n3 records");
            formatik.Separators.Should().NotBeNullOrEmpty("there should be separator");
            formatik.Separators.Count().Should().Be(2, "because output example is a table");
            formatik.Separators.First().Should().Be("\n", "separator is a new line");
            formatik.Separators.Skip(1).First().Should().Be(",", "element is a \",\" comma");
            formatik.Tokens.Should().NotBeNull("there should be tokens");
            formatik.Tokens.Count().Should().Be(4, "there should be 4 token");

            var token = formatik.Tokens.ElementAt(0);
            token.InputSelector.Should().Be("_id", "token is _id");
            token.OutputSelector.Should().Be("0", "this is the first element of the output table");
            token.Prefix.Should().BeEmpty("ID token's prefix should be empty");
            token.Suffix.Should().BeEmpty("ID token's suffix should be empty");

            token = formatik.Tokens.ElementAt(1);
            token.InputSelector.Should().Be("_date", "token is _date");
            token.OutputSelector.Should().Be("1", "this is the second element of the output table");
            token.Prefix.Should().BeEmpty("Date token's prefix should be empty");
            token.Suffix.Should().BeEmpty("Date token's suffix should be empty");

            token = formatik.Tokens.ElementAt(2);
            token.InputSelector.Should().Be("configuration.accountId", "token is configuration.accountId");
            token.OutputSelector.Should().Be("2", "this is the third element of the output table");
            token.Prefix.Should().BeEmpty("configuration.accountId token's prefix should be empty");
            token.Suffix.Should().BeEmpty("configuration.accountId token's suffix should be empty");

            token = formatik.Tokens.ElementAt(3);
            token.InputSelector.Should().Be("seller.countryCode", "token is seller.countryCode");
            token.OutputSelector.Should().Be("3", "this is the third element of the output table");
            token.Prefix.Should().BeEmpty("seller.countryCode token's prefix should be empty");
            token.Suffix.Should().BeEmpty("seller.countryCode token's suffix should be empty");

            // validate processing
            var processed = formatik.Process(input, Encoding.ASCII);
            var expectedOutput = File.ReadAllText("../../../TestData/2.json-csv.1/output.csv");

            processed.Should().Be(expectedOutput.Replace("\r", ""), "processed output should be equal to expected output");
        }

        [Fact]
        public void json_csv_2()
        {
            var input = File.ReadAllText("../../../TestData/2.json-csv.2/input.json").Replace("\r", "");

            var formatik = new Formatik(
                input,
                File.ReadAllText("../../../TestData/2.json-csv.2/example.csv").Replace("\r", ""));

            formatik.Should().NotBeNull();
            formatik.Header.Should().Be("ID,timestamp,accountId,country\n\"");
            formatik.Footer.Should().Be("\"\n3 records");
            formatik.Separators.Should().NotBeNullOrEmpty("there should be separator");
            formatik.Separators.Count().Should().Be(2, "because output example is a table");
            formatik.Separators.First().Should().Be("\"\n\"", "separator is a new line with ending and beginning quotes");
            formatik.Separators.Skip(1).First().Should().Be(",", "element is a \",\" comma");
            formatik.Tokens.Should().NotBeNull("there should be tokens");
            formatik.Tokens.Count().Should().Be(4, "there should be 4 token");

            // ID
            var token = formatik.Tokens.ElementAt(0);
            token.InputSelector.Should().Be("_id", "token is _id");
            token.OutputSelector.Should().Be("0", "this is the first element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().Be("\"", "output values are wrapped in quotes");

            // _date
            token = formatik.Tokens.ElementAt(1);
            token.InputSelector.Should().Be("_date", "token is _date");
            token.OutputSelector.Should().Be("1", "this is the second element of the output table");
            token.Prefix.Should().Be("\"", "output values are wrapped in quotes");
            token.Suffix.Should().Be("\"", "output values are wrapped in quotes");

            // accountId
            token = formatik.Tokens.ElementAt(2);
            token.InputSelector.Should().Be("configuration.accountId", "token is configuration.accountId");
            token.OutputSelector.Should().Be("2", "this is the third element of the output table");
            token.Prefix.Should().Be("", "output value is numeric");
            token.Suffix.Should().Be("", "output value is numeric");

            // country
            token = formatik.Tokens.ElementAt(3);
            token.InputSelector.Should().Be("seller.countryCode", "token is seller.countryCode");
            token.OutputSelector.Should().Be("3", "this is the third element of the output table");
            token.Prefix.Should().Be("\"", "output values are wrapped in quotes");
            token.Suffix.Should().BeEmpty();

            // validate processing
            var processed = formatik.Process(input, Encoding.ASCII);
            var expectedOutput = File.ReadAllText("../../../TestData/2.json-csv.2/output.csv");

            processed.Should().Be(expectedOutput.Replace("\r", ""), "processed output should be equal to expected output");
        }

        [Fact]
        public void json_csv_3()
        {
            var input = File.ReadAllText("../../../TestData/2.json-csv.3/input.json").Replace("\r", "");

            var formatik = new Formatik(
                input,
                File.ReadAllText("../../../TestData/2.json-csv.3/example.csv").Replace("\r", ""));

            formatik.Should().NotBeNull();
            formatik.Header.Should().Be("ID,accountId,timestamp,country\n\"");
            formatik.Footer.Should().Be("\"\n3 records");
            formatik.Separators.Should().NotBeNullOrEmpty("there should be separator");
            formatik.Separators.Count().Should().Be(2, "because output example is a table");
            formatik.Separators.First().Should().Be("\"\n\"", "separator is a new line with ending and beginning quotes");
            formatik.Separators.Skip(1).First().Should().Be(",", "element is a \",\" comma");
            formatik.Tokens.Should().NotBeNull("there should be tokens");
            formatik.Tokens.Count().Should().Be(4, "there should be 4 token");

            // ID
            var token = formatik.Tokens.ElementAt(0);
            token.InputSelector.Should().Be("_id", "token is _id");
            token.OutputSelector.Should().Be("0", "this is the first element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().Be("\"", "output values are wrapped in quotes");

            // accountId
            token = formatik.Tokens.ElementAt(1);
            token.InputSelector.Should().Be("configuration.accountId", "token is configuration.accountId");
            token.OutputSelector.Should().Be("1", "this is the second element of the output table");
            token.Prefix.Should().Be("", "output value is numeric");
            token.Suffix.Should().Be("", "output value is numeric");

            // _date
            token = formatik.Tokens.ElementAt(2);
            token.InputSelector.Should().Be("_date", "token is _date");
            token.OutputSelector.Should().Be("2", "this is the third element of the output table");
            token.Prefix.Should().Be("\"", "output values are wrapped in quotes");
            token.Suffix.Should().Be("\"", "output values are wrapped in quotes");

            // country
            token = formatik.Tokens.ElementAt(3);
            token.InputSelector.Should().Be("seller.countryCode", "token is seller.countryCode");
            token.OutputSelector.Should().Be("3", "this is the third element of the output table");
            token.Prefix.Should().Be("\"", "output values are wrapped in quotes");
            token.Suffix.Should().BeEmpty();

            // validate processing
            var processed = formatik.Process(input, Encoding.ASCII);
            var expectedOutput = File.ReadAllText("../../../TestData/2.json-csv.3/output.csv");

            processed.Should().Be(expectedOutput.Replace("\r", ""), "processed output should be equal to expected output");
        }

        [Fact]
        public void json_csv_4()
        {
            var input = File.ReadAllText("../../../TestData/2.json-csv.4/input.json").Replace("\r", "");

            var formatik = new Formatik(
                input,
                File.ReadAllText("../../../TestData/2.json-csv.4/example.csv").Replace("\r", ""));

            formatik.Should().NotBeNull();
            formatik.Header.Should().BeEmpty();
            formatik.Footer.Should().BeEmpty();
            formatik.Separators.Should().NotBeNullOrEmpty("there should be separator");
            formatik.Separators.Count().Should().Be(2, "because output example is a table");
            formatik.Separators.First().Should().Be("\n", "separator is a new line with ending and beginning quotes");
            formatik.Separators.Skip(1).First().Should().Be(",", "element is a \",\" comma");
            formatik.Tokens.Should().NotBeNull("there should be tokens");
            formatik.Tokens.Count().Should().Be(4, "there should be 4 token");

            // ID
            var token = formatik.Tokens.ElementAt(0);
            token.InputSelector.Should().Be("_id", "token is _id");
            token.OutputSelector.Should().Be("0", "this is the first element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().BeEmpty();

            // _date
            token = formatik.Tokens.ElementAt(1);
            token.InputSelector.Should().Be("_date", "token is _date");
            token.OutputSelector.Should().Be("1", "this is the third element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().BeEmpty();

            // accountId
            token = formatik.Tokens.ElementAt(2);
            token.InputSelector.Should().Be("configuration.accountId", "token is configuration.accountId");
            token.OutputSelector.Should().Be("2", "this is the second element of the output table");
            token.Prefix.Should().Be("", "output value is numeric");
            token.Suffix.Should().Be("", "output value is numeric");

            // country
            token = formatik.Tokens.ElementAt(3);
            token.InputSelector.Should().Be("seller.countryCode", "token is seller.countryCode");
            token.OutputSelector.Should().Be("3", "this is the third element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().BeEmpty();

            // validate processing
            var processed = formatik.Process(input, Encoding.ASCII);
            var expectedOutput = File.ReadAllText("../../../TestData/2.json-csv.4/output.csv");

            processed.Should().Be(expectedOutput.Replace("\r", ""), "processed output should be equal to expected output");
        }

        [Fact]
        public void json_csv_5()
        {
            var input = File.ReadAllText("../../../TestData/2.json-csv.5/input.json").Replace("\r", "");

            var formatik = new Formatik(
                input,
                File.ReadAllText("../../../TestData/2.json-csv.5/example.csv").Replace("\r", ""));

            formatik.Should().NotBeNull();
            formatik.Header.Should().Be("ID,timestamp,accountId,country\n");
            formatik.Footer.Should().BeEmpty();
            formatik.Separators.Should().NotBeNullOrEmpty("there should be separator");
            formatik.Separators.Count().Should().Be(2, "because output example is a table");
            formatik.Separators.First().Should().Be("\n", "separator is a new line with ending and beginning quotes");
            formatik.Separators.Skip(1).First().Should().Be(",", "element is a \",\" comma");
            formatik.Tokens.Should().NotBeNull("there should be tokens");
            formatik.Tokens.Count().Should().Be(4, "there should be 4 token");

            // ID
            var token = formatik.Tokens.ElementAt(0);
            token.InputSelector.Should().Be("_id", "token is _id");
            token.OutputSelector.Should().Be("0", "this is the first element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().BeEmpty();

            // _date
            token = formatik.Tokens.ElementAt(1);
            token.InputSelector.Should().Be("_date", "token is _date");
            token.OutputSelector.Should().Be("1", "this is the third element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().BeEmpty();

            // accountId
            token = formatik.Tokens.ElementAt(2);
            token.InputSelector.Should().Be("configuration.accountId", "token is configuration.accountId");
            token.OutputSelector.Should().Be("2", "this is the second element of the output table");
            token.Prefix.Should().Be("", "output value is numeric");
            token.Suffix.Should().Be("", "output value is numeric");

            // country
            token = formatik.Tokens.ElementAt(3);
            token.InputSelector.Should().Be("seller.countryCode", "token is seller.countryCode");
            token.OutputSelector.Should().Be("3", "this is the third element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().BeEmpty();

            // validate processing
            var processed = formatik.Process(input, Encoding.ASCII);
            var expectedOutput = File.ReadAllText("../../../TestData/2.json-csv.5/output.csv");

            processed.Should().Be(expectedOutput.Replace("\r", ""), "processed output should be equal to expected output");
        }

        [Fact]
        public void json_csv_6()
        {
            var input = File.ReadAllText("../../../TestData/2.json-csv.6/input.json").Replace("\r", "");

            var formatik = new Formatik(
                input,
                File.ReadAllText("../../../TestData/2.json-csv.6/example.csv").Replace("\r", ""));

            formatik.Should().NotBeNull();
            formatik.Header.Should().Be("accountId,country,id,timestamp\n");
            formatik.Footer.Should().Be("\"");
            formatik.Separators.Should().NotBeNullOrEmpty("there should be separator");
            formatik.Separators.Count().Should().Be(2, "because output example is a table");
            formatik.Separators.First().Should().Be("\"\n", "separator is a new line with ending and beginning quotes");
            formatik.Separators.Skip(1).First().Should().Be(",\"", "element is a \",\\\" comma");
            formatik.Tokens.Should().NotBeNull("there should be tokens");
            formatik.Tokens.Count().Should().Be(4, "there should be 4 token");

            // accountId
            var token = formatik.Tokens.ElementAt(0);
            token.InputSelector.Should().Be("configuration.accountId", "token is configuration.accountId");
            token.OutputSelector.Should().Be("0", "this is the second element of the output table");
            token.Prefix.Should().Be("", "output value is numeric");
            token.Suffix.Should().Be("", "output value is numeric");

            // country
            token = formatik.Tokens.ElementAt(1);
            token.InputSelector.Should().Be("seller.countryCode", "token is seller.countryCode");
            token.OutputSelector.Should().Be("1", "this is the third element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().Be("\"");

            // ID
            token = formatik.Tokens.ElementAt(2);
            token.InputSelector.Should().Be("_id", "token is _id");
            token.OutputSelector.Should().Be("2", "this is the first element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().Be("\"");

            // _date
            token = formatik.Tokens.ElementAt(3);
            token.InputSelector.Should().Be("_date", "token is _date");
            token.OutputSelector.Should().Be("3", "this is the third element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().BeEmpty();

            // validate processing
            var processed = formatik.Process(input, Encoding.ASCII);
            var expectedOutput = File.ReadAllText("../../../TestData/2.json-csv.6/output.csv");

            processed.Should().Be(expectedOutput.Replace("\r", ""), "processed output should be equal to expected output");
        }

        [Fact]
        public void json_csv_7()
        {
            var input = File.ReadAllText("../../../TestData/2.json-csv.7/input.json").Replace("\r", "");

            var formatik = new Formatik(
                input,
                File.ReadAllText("../../../TestData/2.json-csv.7/example.csv").Replace("\r", ""));

            formatik.Should().NotBeNull();
            formatik.Header.Should().BeEmpty();
            formatik.Footer.Should().Be("\"");
            formatik.Separators.Should().NotBeNullOrEmpty("there should be separator");
            formatik.Separators.Count().Should().Be(2, "because output example is a table");
            formatik.Separators.First().Should().Be("\"\n", "separator is a new line with ending and beginning quotes");
            formatik.Separators.Skip(1).First().Should().Be(",\"", "element is a \",\\\" comma");
            formatik.Tokens.Should().NotBeNull("there should be tokens");
            formatik.Tokens.Count().Should().Be(2, "there should be 2 token");

            // ID
            var token = formatik.Tokens.ElementAt(0);
            token.InputSelector.Should().Be("_id", "token is _id");
            token.OutputSelector.Should().Be("0", "this is the first element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().BeEmpty();

            // sellers
            token = formatik.Tokens.ElementAt(1);
            token.InputSelector.Should().Be("configuration.config.rules[0].actions.online[0]", "token is sellers rule");
            token.OutputSelector.Should().Be("1", "this is the third element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().BeEmpty();

            // validate processing
            var processed = formatik.Process(input, Encoding.ASCII);
            var expectedOutput = File.ReadAllText("../../../TestData/2.json-csv.7/output.csv");

            processed.Should().Be(expectedOutput.Replace("\r", ""), "processed output should be equal to expected output");
        }        

        [Fact]
        public void json_json_1()
        {
            var input = File.ReadAllText("../../../TestData/3.json-json.1/input.json").Replace("\r", "");

            var formatik = new Formatik(
                input,
                File.ReadAllText("../../../TestData/3.json-json.1/example.json").Replace("\r", ""));

            formatik.Should().NotBeNull();
            formatik.Header.Should().Be("[\n    {\n        \"id\": \"");
            formatik.Footer.Should().Be("\"\n    }    \n]");
            formatik.Separators.Should().NotBeNullOrEmpty("there should be separator");
            formatik.Separators.Count().Should().Be(2, "because output example is a table");
            formatik.Separators.First().Should().Be("\"\n    },\n    {\n        \"id\": \"");
            formatik.Separators.Skip(1).First().Should().Be(",\n        \"");
            formatik.Tokens.Should().NotBeNull("there should be tokens");
            formatik.Tokens.Count().Should().Be(4, "there should be 4 token");

            // _id
            var token = formatik.Tokens.ElementAt(0);
            token.InputSelector.Should().Be("_id", "token is _id");
            token.OutputSelector.Should().Be("0", "this is the first element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().Be("\"");

            // _date
            token = formatik.Tokens.ElementAt(1);
            token.InputSelector.Should().Be("_date", "token is _date");
            token.OutputSelector.Should().Be("1", "this is the third element of the output table");
            token.Prefix.Should().Be("date\": \"");
            token.Suffix.Should().Be("\"");

            // accountId
            token = formatik.Tokens.ElementAt(2);
            token.InputSelector.Should().Be("configuration.accountId", "token is configuration.accountId");
            token.OutputSelector.Should().Be("2", "this is the second element of the output table");
            token.Prefix.Should().Be("accountId\": ", "output value is numeric");
            token.Suffix.Should().Be("", "output value is numeric");

            // country
            token = formatik.Tokens.ElementAt(3);
            token.InputSelector.Should().Be("seller.countryCode", "token is seller.countryCode");
            token.OutputSelector.Should().Be("3", "this is the third element of the output table");
            token.Prefix.Should().Be("countryCode\": \"");
            token.Suffix.Should().BeEmpty();

            // validate processing
            var processed = formatik.Process(input, Encoding.ASCII);
            var expectedOutput = File.ReadAllText("../../../TestData/3.json-json.1/output.json");

            processed.Should().Be(expectedOutput.Replace("\r", ""), "processed output should be equal to expected output");
        }

        [Fact]
        public void json_tsql_1()
        {
            var input = File.ReadAllText("../../../TestData/4.json-tsql.1/input.json").Replace("\r", "");

            var formatik = new Formatik(
                input,
                File.ReadAllText("../../../TestData/4.json-tsql.1/example.sql").Replace("\r", ""));

            formatik.Should().NotBeNull();
            formatik.Header.Should().Be("DECLARE @myTable TABLE (\n    [id] VARCHAR(MAX) NOT NULL PRIMARY KEY,\n    [timestamp] DATETIME NOT NULL,\n    [accountId] INT NOT NULL,\n    [countryCode] VARCHAR(MAX) NOT NULL\n)\n\nINSERT INTO @myTable VALUES (\"");
            formatik.Footer.Should().Be("\");\n\nSELECT  countryCode,\n        COUNT(*)\nFROM @myTable T\nGROUP BY T.countryCode\n");
            formatik.Separators.Should().NotBeNullOrEmpty("there should be separator");
            formatik.Separators.Count().Should().Be(2, "because output example is a table");
            formatik.Separators.First().Should().Be("\");\nINSERT INTO @myTable VALUES (\"");
            formatik.Separators.Skip(1).First().Should().Be(", ");
            formatik.Tokens.Should().NotBeNull("there should be tokens");
            formatik.Tokens.Count().Should().Be(4, "there should be 4 token");

            // _id
            var token = formatik.Tokens.ElementAt(0);
            token.InputSelector.Should().Be("_id", "token is _id");
            token.OutputSelector.Should().Be("0", "this is the first element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().Be("\"");

            // _date
            token = formatik.Tokens.ElementAt(1);
            token.InputSelector.Should().Be("_date", "token is _date");
            token.OutputSelector.Should().Be("1", "this is the third element of the output table");
            token.Prefix.Should().Be("\"");
            token.Suffix.Should().Be("\"");

            // accountId
            token = formatik.Tokens.ElementAt(2);
            token.InputSelector.Should().Be("configuration.accountId", "token is configuration.accountId");
            token.OutputSelector.Should().Be("2", "this is the second element of the output table");
            token.Prefix.Should().BeEmpty();
            token.Suffix.Should().BeEmpty();

            // country
            token = formatik.Tokens.ElementAt(3);
            token.InputSelector.Should().Be("seller.countryCode", "token is seller.countryCode");
            token.OutputSelector.Should().Be("3", "this is the third element of the output table");
            token.Prefix.Should().Be("\"");
            token.Suffix.Should().BeEmpty();

            // validate processing
            var processed = formatik.Process(input, Encoding.ASCII);
            var expectedOutput = File.ReadAllText("../../../TestData/4.json-tsql.1/output.sql");

            processed.Should().Be(expectedOutput.Replace("\r", ""), "processed output should be equal to expected output");
        }
    }
}
