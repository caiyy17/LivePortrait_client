import json

# 读取txt文件
path = 'Assets/.server/blendshape.txt'
out_path = 'Assets/LivePortrait/blendshape.json'
data = open(path).read()

# 定义函数将文本转换为嵌套字典
def parse_to_dict(data):

    results = []
    lines = data.strip().split('\n')

    result = {}
    for line in lines:
        line = line.strip()
        if line.startswith("Frame"):
            frame_info = line.split("(Time:")
            frame_number = frame_info[0].strip().replace("Frame ", "")
            time_value = frame_info[1].strip().replace(")", "")
            result["Frame"] = int(frame_number)
            result["Time"] = time_value
        elif line == "":
            results.append(result)
            result = {}
        elif line.endswith(":"):
            pass
        else:
            key, value = line.split(":")
            result[key.strip()] = float(value.strip())

    return results

# 将数据解析为字典
parsed_data = parse_to_dict(data)

# 将字典转换为JSON
json_data = json.dumps(parsed_data[0], indent=2)

# 输出JSON数据
print(json_data)
# save
with open(out_path, 'w') as f:
    f.write(json_data)
