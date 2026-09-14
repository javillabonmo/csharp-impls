provider "aws" {
  region = "us-east-1"
}

data "aws_ami" "ubuntu26" {
  most_recent = true

  filter {
    name   = "name"
    values = ["ubuntu/images/hvm-ssd-gp3/ubuntu-resolute-26.04-amd64-server-*"]
  }

  owners = ["099720109477"] # Canonical
}
#type de recurso y nombre del recurso
# "Together, the resource type and resource name 
#form a unique resource address for the resource in your configuration"
resource "aws_instance" "ec2-personalblog" {
  ami           = data.aws_ami.ubuntu26.id
  instance_type = var.instance_type

  vpc_security_group_ids = [aws_security_group.personalblog-sg.id]
  subnet_id              = aws_subnet.tbrzc-subnet-public-1a.id

  key_name = aws_key_pair.personalblog.key_name


  tags = {
    Name = var.instance_name
  }
}

resource "aws_key_pair" "personalblog" {
  key_name   = "personalblog-key"
  public_key = file("~/.ssh/personalblog-key.pub")
}

resource "aws_internet_gateway" "tbrzc-igw" {
  vpc_id = module.tbrzc.vpc_id

  tags = {
    Name = "igw-principal"
  }
}

resource "aws_subnet" "tbrzc-subnet-public-1a" {
  vpc_id            = module.tbrzc.vpc_id
  cidr_block        = "10.0.0.0/25"
  availability_zone = "us-east-1a"

  map_public_ip_on_launch = true

  tags = {
    Name = "public-subnet-1a"
  }
}

resource "aws_subnet" "tbrzc-subnet-private-1b" {
  vpc_id            = module.tbrzc.vpc_id
  cidr_block        = "10.0.0.128/25"
  availability_zone = "us-east-1b"

  tags = {
    Name = "private-subnet-1b"
  }
}

resource "aws_route_table" "tbrzc-public-route-table" {
  vpc_id = module.tbrzc.vpc_id

  tags = {
    Name = "public-rt"
  }
}
resource "aws_route" "public_internet_access" {
  route_table_id         = aws_route_table.tbrzc-public-route-table.id
  destination_cidr_block = "0.0.0.0/0"
  gateway_id             = aws_internet_gateway.tbrzc-igw.id
}
resource "aws_route_table_association" "tbrzc-public-route-table-association" {
  subnet_id      = aws_subnet.tbrzc-subnet-public-1a.id
  route_table_id = aws_route_table.tbrzc-public-route-table.id
}

resource "aws_route_table" "tbrzc-private-route-table" {
  vpc_id = module.tbrzc.vpc_id

  tags = {
    Name = "private-rt"
  }
}

resource "aws_route_table_association" "tbrzc-private-route-table-association" {
  subnet_id      = aws_subnet.tbrzc-subnet-private-1b.id
  route_table_id = aws_route_table.tbrzc-private-route-table.id
}


resource "aws_vpc_security_group_ingress_rule" "allow_ssh" {
  security_group_id = aws_security_group.personalblog-sg.id
  cidr_ipv4         = "0.0.0.0/0" # my public IP
  from_port         = 22
  ip_protocol       = "tcp"
  to_port           = 22
  description       = "Allow SSH from my IP"
}

resource "aws_vpc_security_group_ingress_rule" "allow_http" {
  security_group_id = aws_security_group.personalblog-sg.id
  cidr_ipv4         = "0.0.0.0/0"
  from_port         = 8080
  ip_protocol       = "tcp"
  to_port           = 8080
  description       = "Allow HTTP traffic"
}



resource "aws_security_group" "personalblog-sg" {
  name        = "personalblog-sg"
  description = "Security group for web and administrative access"
  vpc_id      = module.tbrzc.vpc_id

  tags = {
    Name = "personalblog-sg"
  }
}


resource "aws_vpc_security_group_egress_rule" "allow_all_traffic_ipv4" {
  security_group_id = aws_security_group.personalblog-sg.id
  cidr_ipv4         = "0.0.0.0/0"
  ip_protocol       = "-1" # Semantic equivalent to "all protocols"
  description       = "Allow all outbound traffic"
}


#terraform resuelve automaticamente las dependencias entre recursos
#asi que no es necesario declarar dependencias entre recursos NI organizar en orden
# pero organiza para mantener legibilidad
module "tbrzc" { # en este caso vpc es nombre unico del modulo
  source  = "terraform-aws-modules/vpc/aws"
  version = "6.7.2"

  name = "tbrzc-vpc"
  cidr = "10.0.0.0/24" #256 ips

  azs = ["us-east-1a", "us-east-1b"]

  tags = {
    Name = "main-vpc"
  }
}
